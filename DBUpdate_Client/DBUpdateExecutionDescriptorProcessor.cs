using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorProcessor
    {
        private readonly ILogger logger;
        private readonly DBUpdateExecutionDescriptor executionDescriptor;
        private readonly IConfigurationProvider configurationProvider;
        private readonly DBUpdateConfiguration configuration;
        private readonly IConnectionProvider connectionProvider;

        public DBUpdateExecutionDescriptorProcessor(ILogger logger, DBUpdateExecutionDescriptor executionDescriptor, IConfigurationProvider configurationProvider, DBUpdateConfiguration configuration)
        {
            this.logger = logger;
            this.executionDescriptor = executionDescriptor;
            this.configurationProvider = configurationProvider;
            this.connectionProvider = new ConstantConnectionProvider(configurationProvider.GetConnectionString(executionDescriptor.ConnectionStringName));
            this.configuration = configuration;
        }

        public void Process()
        {
            Log($"Processing {executionDescriptor.Path}");

            Log($"Using connection string {executionDescriptor.ConnectionStringName}");

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
            blocksToExecute = RemoveBlocksAlreadyExecuted(blocksToExecute, connectionProvider);

            // For each block not run yet
            foreach (var block in blocksToExecute)
            {
                Log($"Executing block {block}");

                // For each script to execute
                var scripts = block.Scripts.Select(sn => Path.Combine(configuration.WorkingDirectory, sn.Name));
                foreach (var script in scripts)
                {
                    Log($"Executing script {script}");
                    // Read the script

                    // Parse into batches according to GO
                    var batches = new DBUpdateFileScriptToBatch().GetScriptAndSplit(script);

                    // For each batch
                    foreach (var batch in batches)
                    {
                        // Execute the batch
                        ExecuteBatch(batch);
                    }

                    // Update the DB to indicate that the script has been executed (incl. block details)
                    LogScriptExecution(connectionProvider, block.Name, script, run.Id);
                }
            }

            // Update the Run in DB
            run.Close();
        }
        private void Log(string message) => this.logger?.LogMessage(message);
        private void CheckDBStructure(IConnectionProvider connectionProvider) => new DBUpdateStructureValidator(connectionProvider).EnsureStructureExists();
        private IEnumerable<DBUpdateExecutionBlockDescriptor> RemoveBlocksAlreadyExecuted(IEnumerable<DBUpdateExecutionBlockDescriptor> blocksToExecute, IConnectionProvider connectionProvider)
        {
            ScriptGateway scriptGateway = new ScriptGateway(connectionProvider);
            var executedBlockNames = scriptGateway.GetExecutedScriptNames().Select(sn => blocksToExecute.FirstOrDefault(bte => bte.Name == sn)).Where(b => b != null);

            return blocksToExecute.Except(executedBlockNames).ToArray();
        }


        // TODO: Ici qu'il y a les batchs
        //private IEnumerable<IEnumerable<string>> SplitScriptIntoBatches(IEnumerable<string> scriptText)
        //{
        //    //DBUpdateScriptToBatch dBUpdateScriptToBatch = new DBUpdateScriptToBatch();
        //    //return (IEnumerable<IEnumerable<string>>)dBUpdateScriptToBatch;

        //    var result = new List<IEnumerable<string>>();
        //    IList<string> currentBatch = new List<string>();

        //    bool add = true;

        //    foreach (string line in scriptText)
        //    {
        //        bool end = false;

        //        if (line.StartsWith("/*"))
        //        {
        //            add = false;
        //        }
        //        if (line.StartsWith("*/"))
        //        {
        //            add = true;
        //            end = true;
        //        }

        //        if (add && !end)
        //        {
        //            if (line == "GO")
        //            {
        //                result.Add(currentBatch);
        //                currentBatch = new List<string>();
        //            }
        //            else
        //            {
        //                if (CheckBatchIsValid(line))
        //                {
        //                    currentBatch.Add(line);
        //                }
        //            }
        //    }
        //}
        //if (currentBatch.Count > 0)
        //{
        //    result.Add(currentBatch);
        //}
        //return result;
        //}

        private void ExecuteBatch(IEnumerable<string> batch)
        {
            using (var connection = connectionProvider.GetConnection())
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
        private static void LogScriptExecution(IConnectionProvider connectionProvider, string blockName, string scriptPath, int runId)
        {
            new ScriptGateway(connectionProvider).RecordExecution(runId, blockName, scriptPath);
        }

    }
}
