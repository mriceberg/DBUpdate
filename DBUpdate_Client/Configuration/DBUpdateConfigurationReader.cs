using System;

namespace DBUpdate_Client
{
    public class DBUpdateConfigurationReader
    {
        private readonly IConfigurationProvider configurationProvider;

        public const string WORKING_DIRECTORY_APPSETTING_NAME = "WorkingDirectory";
        public const string XSD_NAME_APPSETTING_NAME = "XsdName";
        public const string FILE_LOGGER = "FileLogger";
        public const string CONSOLE_LOGGER = "ConsoleLogger";

        public DBUpdateConfigurationReader(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public DBUpdateConfiguration Read()
        {
            string workingDirectory = ReadWorkingDirectory();
            string xsdName = ReadXsdName();
            bool fileLogger = ReadFileLogger();
            bool consoleLogger = ReadConsoleLogger();
            return new DBUpdateConfigurationBuilder()
                .SetWorkingDirectory(workingDirectory)
                .SetXsdName(xsdName)
                .SetFileLogger(fileLogger)
                .SetConsoleLogger(consoleLogger)
                .Build();
        }

        private string ReadXsdName() => this.configurationProvider.GetAppSetting(XSD_NAME_APPSETTING_NAME);
        private string ReadWorkingDirectory() => this.configurationProvider.GetAppSetting(WORKING_DIRECTORY_APPSETTING_NAME);
        private bool ReadFileLogger() => bool.Parse(this.configurationProvider.GetAppSetting(FILE_LOGGER));
        private bool ReadConsoleLogger() => bool.Parse(this.configurationProvider.GetAppSetting(CONSOLE_LOGGER));
    }
}
