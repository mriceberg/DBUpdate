using System;
using System.Collections.Generic;
using System.IO;

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
            ILogger logger = null;

            _config = new DBUpdateConfigurationReader(configurationProvider).Read();
            _parameters = new DBUpdateParametersReader(args).Read;

            if (!String.IsNullOrEmpty(_parameters.IsScan))
            {
                AddMissingSqlFilesInXml addMissingSqlFilesInXml = new AddMissingSqlFilesInXml(logger, configurationProvider, _parameters);
                addMissingSqlFilesInXml.AddMissingScriptsInXml();
            }

            DBUpdateExecutionDescriptor executionDescriptor = new DBUpdateExecutionDescriptorReader().Read("C:/temp/workingdir/ScriptsEtt.xml");
            StreamWriter myLogFile = null;

            DefaultLoggerFactory logFactory = new DefaultLoggerFactory();
            ILogger consoleLog = null;

            if (!_parameters.IsSilent)
            {
              consoleLog = _config.ConsoleLogger ? logFactory.MakeConsoleLogger() : null;
            }

            try
            {
                if (_config.FileLogger)
                {
                    if (_parameters.IsAppend)
                    {
                        myLogFile = File.AppendText(@"C:\temp\workingdir\log.txt");
                    }
                    else
                    {
                        myLogFile = File.CreateText(@"C:\temp\workingdir\log.txt");
                    }

                    logger = logFactory.MakeFileLogger(myLogFile);
                }
                logger = logFactory.MakeMultiCastLogger(consoleLog, logger);

                logger.LogMessage(_parameters.IsUpToBlock);
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
                    if (!_parameters.IsSilent)
                    {
                        Console.WriteLine("Hit enter to stop the program");
                        Console.ReadLine();
                    }
                }

                logger.LogMessage("Fin de l'exécution \n");
            }
            finally
            {
                if (myLogFile != null) myLogFile.Dispose();
            }
        }
    }
}

