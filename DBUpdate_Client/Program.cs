using System;

namespace DBUpdate_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            UtilFactory utils = new DefaultUtilFactory();
            ConfigurationProvider configuration = utils.MakeConfigurationProvider();
            Logger logger = utils.MakeLogger();

            DBUpdateController controller = new DBUpdateController(configuration, logger);

            Console.ReadLine();
        }


    }
}
