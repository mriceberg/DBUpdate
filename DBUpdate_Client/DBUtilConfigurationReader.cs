namespace DBUpdate_Client
{
    public class DBUtilConfigurationReader
    {
        private readonly ConfigurationProvider configurationProvider;

        public const string WORKING_DIRECTORY_APPSETTING_NAME = "WorkingDirectory";

        public DBUtilConfigurationReader(ConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public DBUtilConfiguration Read()
        {
            string workingDirectory = ReadWorkingDirectory();

            return new DBUtilConfigurationBuilder()
                .SetWorkingDirectory(workingDirectory)
                .Build();
        }

        public string ReadWorkingDirectory() => this.configurationProvider.GetAppSetting(WORKING_DIRECTORY_APPSETTING_NAME);
    }
}
