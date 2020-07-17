using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateController
    {
        private readonly ConfigurationProvider configurationProvider;
        private readonly Logger logger;

        private DBUpdateConfiguration configuration;

        public DBUpdateController(ConfigurationProvider configuration, Logger logger)
        {
            this.configurationProvider = configuration;
            this.logger = logger;
        }

        public void Execute()
        {
            this.configuration = ReadConfiguration();
            Log($"Working directory: {configuration.WorkingDirectory}");

            // Get list of files to process
            var executionDescriptors = ReadExecutionDescriptors();
            ProcessExecutionDescriptors(executionDescriptors);
        }

        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this.configurationProvider).Read();
        private IEnumerable<DBUpdateExecutionDescriptor> ReadExecutionDescriptors() => new DBUpdateExecutionDescriptorReader().ReadAll(this.configuration.WorkingDirectory);
        private void ProcessExecutionDescriptors(IEnumerable<DBUpdateExecutionDescriptor> executionDescriptors)
        {
            foreach (var executionDescriptor in executionDescriptors)
            {
                try
                {
                    ProcessExecutionDescriptor(executionDescriptor);
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        Log(ex.ToString());
                        ex = ex.InnerException;
                    }
                }
            }
        }
        private void ProcessExecutionDescriptor(DBUpdateExecutionDescriptor executionDescriptor)
        {
            Log($"Processing {executionDescriptor.Path}");

            // Check if the configuration file contains a matching connection string
            var connectionString = GetConnectionString(executionDescriptor.ConnectionStringName);
            Log($"Using connection string {executionDescriptor.ConnectionStringName}");

            // If it exists
            if (ConnectionStringIsValid(connectionString))
            {

                ConnectionProvider connectionProvider = new ConstantConnectionProvider(connectionString);

                Log("Checking DB structure");
                // Check if the structure exists
                CheckDBStructure(connectionProvider);

                // Create the Run in DB
                RunGateway runGateway = new RunGateway(connectionProvider);
                DBUpdateRun run = new DBUpdateRun(runGateway);
                run.Create();

                Log($"Run {run.Id} created.");

                // Read the list of blocks required to reach the expected state in the correct order
                var blocksToExecute = executionDescriptor.BlocksToExecute;

                // Check in DB until which blocks scripts have already been run
                blocksToExecute = RemoveBlocksAlreadyExecuted(blocksToExecute, connectionString);

                // For each block not run yet
                foreach (var block in blocksToExecute)
                {
                    Log($"Checking block {block}");
                    // Read the block definition
                    var scripts = block.ScriptNames.Select(sn => Path.Combine(configuration.WorkingDirectory, sn));

                    // For each script to execute
                    foreach (var script in scripts)
                    {
                        Log($"Checking script {script}");
                        // Check that the script is available
                        if (!File.Exists(script))
                        {
                            // Fail if the script is missing
                            throw new FileNotFoundException($"Missing script in block {block}. ", script);
                        }
                    }
                }

                // For each block not run yet
                foreach (var block in blocksToExecute)
                {
                    Log($"Executing block {block}");

                    // For each script to execute
                    var scripts = block.ScriptNames.Select(sn => Path.Combine(configuration.WorkingDirectory, sn));
                    foreach (var script in scripts)
                    {
                        Log($"Executing script {script}");
                        // Read the script
                        string[] scriptText = File.ReadAllLines(script);

                        // Parse into batches according to GO
                        var batches = SplitScriptIntoBatches(scriptText);

                        // For each batch
                        foreach (var batch in batches)
                        {
                            // Execute the batch
                            ExecuteBatch(batch, connectionString);
                        }

                        // Update the DB to indicate that the script has been executed (incl. block details)
                        LogScriptExecution(connectionString, block.Name, script, run.Id);
                    }
                }

                // Update the Run in DB
                run.Close();
            }
            else
            {
                Log($"Connection string {executionDescriptor.ConnectionStringName} not found.");
            }
        }
        private void Log(string message) => this.logger.LogMessage(message);
        private string GetConnectionString(string connectionStringName) => configurationProvider.GetConnectionString(connectionStringName);
        private bool ConnectionStringIsValid(string connectionString) => !String.IsNullOrWhiteSpace(connectionString);
        private static IEnumerable<DBUpdateExecutionBlockDescriptor> RemoveBlocksAlreadyExecuted(IEnumerable<DBUpdateExecutionBlockDescriptor> blocksToExecute, string connectionString)
        {
            IList<DBUpdateExecutionBlockDescriptor> alreadyExecuted = new List<DBUpdateExecutionBlockDescriptor>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = @"SELECT DISTINCT BlockName FROM dbupdate.Script";

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string blockName = reader.GetString(0);
                            var block = blocksToExecute.FirstOrDefault(b => b.Name == blockName);
                            if (block != null)
                            {
                                alreadyExecuted.Add(block);
                            }
                        }
                    }
                }
            }

            return blocksToExecute.Except(alreadyExecuted);
        }
        private static void ExecuteBatch(IEnumerable<string> batch, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = String.Join(Environment.NewLine, batch);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private static void LogScriptExecution(string connectionString, string blockName, string scriptPath, int runId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = @"INSERT INTO dbupdate.Script (RunId, BlockName, ScriptName) VALUES (@RunId, @BlockName, @ScriptName);";
                    command.Parameters.AddWithValue("@RunId", runId);
                    command.Parameters.AddWithValue("@BlockName", blockName);
                    command.Parameters.AddWithValue("@ScriptName", scriptPath);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
        private void CheckDBStructure(ConnectionProvider connectionProvider) => new DBUpdateStructureValidator(connectionProvider).EnsureStructureExists();
        private static IEnumerable<IEnumerable<string>> SplitScriptIntoBatches(IEnumerable<string> scriptText)
        {
            var result = new List<IEnumerable<string>>();
            IList<string> currentBatch = new List<string>();

            foreach (string line in scriptText)
            {
                if (line == "GO")
                {
                    result.Add(currentBatch);
                    currentBatch = new List<string>();
                }
                else
                {
                    currentBatch.Add(line);
                }
            }

            if (currentBatch.Count > 0)
            {
                result.Add(currentBatch);
            }

            return result;
        }
    }
}
