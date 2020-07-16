namespace DBUpdate_Client
{
    public class DBUtilConfigurationBuilder
    {
        private string workingDirectory;

        public DBUtilConfigurationBuilder()
        {
            Reset();
        }

        public DBUtilConfigurationBuilder Reset()
        {
            this.workingDirectory = null;

            return this;
        }

        public DBUtilConfigurationBuilder SetWorkingDirectory(string value)
        {
            this.workingDirectory = value;

            return this;
        }

        public DBUtilConfiguration Build() => new DBUtilConfiguration(workingDirectory);
    }
}
