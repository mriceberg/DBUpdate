using System;
using System.Collections.Generic;

namespace DBUpdate_Client
{
    class Program
    {
        private static DBUpdateParameters _parameters;
        private static DBUpdateConfiguration _config;
        static void Main(string[] args)
        {
            IUtilFactory utils = new DefaultUtilFactory();
            ILoggerFactory loggerFactory = utils.MakeLoggerFactory();
            IConfigurationProvider configurationProvider = utils.MakeConfigurationProvider();
            ILogger logger;
            _config = new DBUpdateConfigurationReader(configurationProvider).Read();
            _parameters = new DBUpdateParametersReader(args).Read;

            //DBUpdateExecutionDescriptor executionDescriptors = new DBUpdateExecutionDescriptorReader().ReadAll(new DBUpdateExecutionDescriptorProvider().GetFilesToRead(_config.WorkingDirectory));
            DBUpdateExecutionDescriptor executionDescriptor = new DBUpdateExecutionDescriptorReader().Read("C:/temp/workingdir/ScriptsEtt.xml");

            if (_parameters.IsSilent)
                logger = loggerFactory.MakeMultiCastLogger(logToConsole: false, logToFile: false);
            else
                logger = loggerFactory.MakeMultiCastLogger(logToConsole: _config.ConsoleLogger, logToFile: _config.FileLogger);

            logger.LogMessage("Starting project");

            if (_parameters.IsTest) 
            {
                // TODO : Créer un DbUpdateCheckParamaters qui va être passé a DbUpdateCheck à la place de _parameters
                DBUpdateCheck check = new DBUpdateCheck(logger, _parameters, configurationProvider, executionDescriptor);
                check.StartTest();
            }
            else
            {
                DBUpdateController controller = new DBUpdateController(configurationProvider, logger, _parameters);
                controller.Execute();
                if (!_parameters.IsSilent) { 
                    Console.WriteLine("Hit enter to stop the program");
                    Console.ReadLine();
                }
            }

            logger.LogMessage("Fin de l'exécution \n");
        }
    }
}

