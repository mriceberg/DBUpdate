using System;

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

            if (_parameters.IsSilent)
                logger = loggerFactory.MakeMultiCastLogger(logToConsole: _config.ConsoleLogger, logToFile: _config.FileLogger);
            else
                logger = loggerFactory.MakeMultiCastLogger(logToConsole: false, logToFile: false);

            logger.LogMessage("Starting project");

            if (_parameters.IsTest) 
            {
                //Créer un DbUpdateCheckParamaters qui va être passé a DbUpdateCheck à la place de _parameters
                DBUpdateCheck check = new DBUpdateCheck(logger, _parameters, configurationProvider);
                check.StartTest();
            }
            else 
            {
                DBUpdateController controller = new DBUpdateController(configurationProvider, logger);
                controller.Execute();
                Console.WriteLine("Hit enter to stop the program");

                Console.ReadLine();
            }

            logger.LogMessage("Fin de l'exécution");
        }
    }
}

