namespace DBUpdate_Client
{
    public class DBUpdateConfigurationReader
    {
        private readonly ConfigurationProvider configurationProvider;

        public const string WORKING_DIRECTORY_APPSETTING_NAME = "WorkingDirectory";

        public DBUpdateConfigurationReader(ConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public DBUpdateConfiguration Read()
        {
            string workingDirectory = ReadWorkingDirectory();

            return new DBUpdateConfigurationBuilder()
                .SetWorkingDirectory(workingDirectory)
                .Build();
        }

        public string ReadWorkingDirectory() => this.configurationProvider.GetAppSetting(WORKING_DIRECTORY_APPSETTING_NAME);
    }
}
