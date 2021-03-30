using System;

namespace DBUpdate_Client
{
    class Program
    {
        private static DBUpdateParameters parameters;
        static void Main(string[] args)
        {
            parameters = new DBUpdateParametersReader(args).Read();
            DBUpdateCheck check = new DBUpdateCheck(parameters.IsTest, parameters.MustCheck);
            check.StartTest();

            //UtilFactory utils = new DefaultUtilFactory();
            //ConfigurationProvider configurationProvider = utils.MakeConfigurationProvider();
            //Logger logger = utils.MakeLogger();

            //DBUpdateController controller = new DBUpdateController(configurationProvider, logger);
            //controller.Execute();

            //Console.ReadLine();
        }
    }
}
