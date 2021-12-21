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
        private readonly DBUpdateParameters parameters;
        public DBUpdateExecutionDescriptorProcessor(ILogger logger, DBUpdateExecutionDescriptor executionDescriptor, IConfigurationProvider configurationProvider, DBUpdateConfiguration configuration, DBUpdateParameters parameters)
        {
            this.logger = logger;
            this.executionDescriptor = executionDescriptor;
            this.configurationProvider = configurationProvider;
            this.connectionProvider = new ConstantConnectionProvider(configurationProvider.GetConnectionString(executionDescriptor.ConnectionStringName));
            this.configuration = configuration;
            this.parameters = parameters;
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

            if (!String.IsNullOrEmpty(parameters.IsUpToBlock) && !String.IsNullOrEmpty(parameters.IsBlockName))
            {
                blocksToExecute = RemoveAllBlocksAndPutSpecificBlockName(blocksToExecute);
            }
            // If the parameter isBlockName is referenced in the parameters then remove all other blockNames and place only this one
            else if ((!String.IsNullOrEmpty(parameters.IsUpToBlock)) && (String.IsNullOrEmpty(parameters.IsBlockName)))
            {
                // Check in DB until which blocks scripts have already been run
                blocksToExecute = RemoveBlocksAlreadyExecuted(blocksToExecute, connectionProvider);
                blocksToExecute = RemoveBlockAfterIsUpTo(blocksToExecute);
            }
            else if (String.IsNullOrEmpty(parameters.IsUpToBlock) && (!String.IsNullOrEmpty(parameters.IsBlockName)))
            {
                blocksToExecute = RemoveAllBlocksAndPutSpecificBlockName(blocksToExecute);
            }
            else
            {
                // Check in DB until which blocks scripts have already been run
                blocksToExecute = RemoveBlocksAlreadyExecuted(blocksToExecute, connectionProvider);
            }

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
                        ExecuteBatch(batch, parameters, script);
                    }

                    // Update the DB to indicate that the script has been executed (incl. block details)
                    LogScriptExecution(connectionProvider, block.Name, script, run.Id);
                }
            }
            run.Close();
        }

        private void Log(string message) => this.logger?.LogMessage(message);
        private void CheckDBStructure(IConnectionProvider connectionProvider) => new DBUpdateStructureValidator(connectionProvider).EnsureStructureExists();
        private IEnumerable<DBUpdateExecutionBlockDescriptor> RemoveBlocksAlreadyExecuted(IEnumerable<DBUpdateExecutionBlockDescriptor> blocksToExecute, IConnectionProvider connectionProvider)
        {
            ScriptGateway scriptGateway = new ScriptGateway(connectionProvider);
            var executedBlockNames = scriptGateway.GetExecutedScriptNames()
                                                  .Select(scriptName => blocksToExecute.FirstOrDefault(blockToExecute => blockToExecute.Name == scriptName))
                                                  .Where(b => (b != null))
                                                  .Where(b => (!this.parameters.IsForce) || (b.Name.ToLower() != this.parameters.IsBlockName.ToLower()));

            return blocksToExecute.Except(executedBlockNames).ToArray();
        }
        private void ExecuteBatch(IEnumerable<string> batch, DBUpdateParameters parameters, string script)
        {
            bool retry = true;

            while (retry)
            {
                try
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
                            retry = false;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Display error message
                    // si on a passé le paramètre qui dit de ne pas s'arrêter en cas d'erreur
                    // poser la question [R]etry ou [C]ancel
                    // si retry alors => retry = true (déjà le cas)
                    // si cancel => throw
                    // si on n'a pas passé le paramètre => throw

                    Log($"Problème d'exécution d'un batch. Exception = { ex.Message}");
                    if (parameters.IsPromptOnError)
                    {
                        char userRetryValue;
                        char v1, v2;
                        
                        do
                        {
                            Console.WriteLine($"Si vous voulez corriger le batch : {batch} dans le script : {script} taper [R]etry or [C]ancel");

                            userRetryValue = char.ToLower(Console.ReadKey().KeyChar);
                            v1 = 'r';
                            v2 = 'c';
                            if (userRetryValue == (v2))
                            {
                                throw;
                            }
                        } while ((userRetryValue != v1) && (userRetryValue != v2));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
        private static void LogScriptExecution(IConnectionProvider connectionProvider, string blockName, string scriptPath, int runId)
        {
            new ScriptGateway(connectionProvider).RecordExecution(runId, blockName, scriptPath);
        }
        private void Test(IEnumerable<DBUpdateExecutionBlockDescriptor> blocks)
        {
            var total = new int[] { 1, 2, 3, 4, 5 }.Aggregate(200, (value, accumulator) => accumulator + value);

            var sum = 0;
            foreach(var value in new int[] { 1, 2, 3, 4, 5 })
            {
                sum += value;
            }

            var myBlocks = blocks.Where(b => b.Scripts.Count() == 2).Select(b => new { Name = b.Name, Script1 = b.Scripts.ElementAt(0), Script2 = b.Scripts.ElementAt(1) });

            var myScripts = myBlocks.Aggregate(new System.Text.StringBuilder(), (sb, block) => sb.AppendLine(block.Script1.Path).AppendLine(block.Script2.Path)).ToString();
        }
        private IEnumerable<DBUpdateExecutionBlockDescriptor> RemoveBlockAfterIsUpTo(IEnumerable<DBUpdateExecutionBlockDescriptor> blocksToExecute)
        {
            return blocksToExecute.TakeWhile(b => b.Name.ToLower() == parameters.IsUpToBlock.ToLower());
        }
        private IEnumerable<DBUpdateExecutionBlockDescriptor> RemoveAllBlocksAndPutSpecificBlockName(IEnumerable<DBUpdateExecutionBlockDescriptor> blocksToExecute)
        {
            return new DBUpdateExecutionBlockDescriptor[] { 
                blocksToExecute.Single(b => b.Name.ToLower() == parameters.IsBlockName.ToLower()) 
            };
        }

    }
}
