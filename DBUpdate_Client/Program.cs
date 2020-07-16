using System;

namespace DBUpdate_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationProvider configuration = new DefaultConfigurationProvider();
            Logger logger = new ConsoleLogger();

            DBUpdateController controller = new DBUpdateController(configuration, logger);

            Console.ReadLine();
        }


    }
}
