using System;

namespace DBUpdate_Client
{
    public class DBUpdateConfigurationReader
    {
        private readonly ConfigurationProvider configurationProvider;

        public const string WORKING_DIRECTORY_APPSETTING_NAME = "WorkingDirectory";
        public const string XSD_NAME_APPSETTING_NAME = "XsdName";


        public DBUpdateConfigurationReader(ConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public DBUpdateConfiguration Read()
        {
            string workingDirectory = ReadWorkingDirectory();
            string xsdName = ReadXsdName();
            return new DBUpdateConfigurationBuilder()
                .SetWorkingDirectory(workingDirectory)
                .SetXsdName(xsdName)
                .Build();
        }

        private string ReadXsdName() => this.configurationProvider.GetAppSetting(XSD_NAME_APPSETTING_NAME);
        public string ReadWorkingDirectory() => this.configurationProvider.GetAppSetting(WORKING_DIRECTORY_APPSETTING_NAME);
    }
}
