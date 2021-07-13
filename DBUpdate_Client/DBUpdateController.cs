using System;
using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateController
    {
        private readonly IConfigurationProvider configurationProvider;
        private readonly ILogger logger;
        private DBUpdateConfiguration configuration;
        private DBUpdateParameters parameters;
        public DBUpdateController(IConfigurationProvider configuration, ILogger logger, DBUpdateParameters parameters)
        {
            this.configurationProvider = configuration;
            this.logger = logger;
            this.parameters = parameters;
        }
        public void Execute()
        {
            this.configuration = ReadConfiguration();
            Log($"Working directory: {configuration.WorkingDirectory}");

            // Get list of files to process
            var executionDescriptors = ReadExecutionDescriptors();
            if (!parameters.IsSimulation) { 
                ProcessExecutionDescriptors(executionDescriptors);
            }
        }

        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this.configurationProvider).Read();
        private IEnumerable<DBUpdateExecutionDescriptor> ReadExecutionDescriptors() => 
            new DBUpdateExecutionDescriptorReader().ReadAll(new DBUpdateExecutionDescriptorProvider().GetFilesToRead(this.configuration.WorkingDirectory));

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
            => new DBUpdateExecutionDescriptorProcessor(this.logger, executionDescriptor, configurationProvider, configuration, parameters).Process();
        private void Log(string message) => this.logger?.LogMessage(message);
    }
}
