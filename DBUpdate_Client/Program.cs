using System;

namespace DBUpdate_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            UtilFactory utils = new DefaultUtilFactory();
            ConfigurationProvider configurationProvider = utils.MakeConfigurationProvider();
            Logger logger = utils.MakeLogger();

            DBUpdateController controller = new DBUpdateController(configurationProvider, logger);
            controller.Execute();

            Console.ReadLine();
        }


    }
}
