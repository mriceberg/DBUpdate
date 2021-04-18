using System;

namespace DBUpdate_Client
{
    class Program
    {
        private static DBUpdateParameters _parameters;
        static void Main(string[] args)
        {
            UtilFactory utils = new DefaultUtilFactory();
            ILogger logger = utils.MakeLogger();
            ConfigurationProvider configurationProvider = utils.MakeConfigurationProvider();

            _parameters = new DBUpdateParametersReader(args).Read;
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
                Console.ReadLine();
            }
            logger.LogMessage("Fin de l'exécution");
        }
    }
}

