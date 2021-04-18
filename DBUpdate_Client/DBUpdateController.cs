using System;
using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateController
    {
        private readonly ConfigurationProvider configurationProvider;
        private readonly ILogger logger;

        private DBUpdateConfiguration configuration;

        public DBUpdateController(ConfigurationProvider configuration, ILogger logger)
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
        private IEnumerable<DBUpdateExecutionDescriptor> ReadExecutionDescriptors() => 
            new DBUpdateExecutionDescriptorReader()
            .ReadAll(new DBUpdateExecutionDescriptorProvider().GetFilesToRead(this.configuration.WorkingDirectory));

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
            => new DBUpdateExecutionDescriptorProcessor(this.logger, executionDescriptor, configurationProvider, configuration).Process();
        private void Log(string message) => this.logger?.LogMessage(message);
    }
}
