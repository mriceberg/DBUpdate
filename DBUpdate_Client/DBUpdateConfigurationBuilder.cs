namespace DBUpdate_Client
{
    public class DBUpdateConfigurationBuilder
    {
        private string workingDirectory;
        private string xsdName;

        public DBUpdateConfigurationBuilder()
        {
            Reset();
        }

        public DBUpdateConfigurationBuilder Reset()
        {
            this.workingDirectory = null;
            this.xsdName = null;

            return this;
        }
        public DBUpdateConfigurationBuilder SetXsdName(string value)
        {
            this.xsdName = value;

            return this;
        }
        public DBUpdateConfigurationBuilder SetWorkingDirectory(string value)
        {
            this.workingDirectory = value;

            return this;
        }

        public DBUpdateConfiguration Build() => new DBUpdateConfiguration(workingDirectory, xsdName);
    }
}
