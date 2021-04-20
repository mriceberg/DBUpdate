namespace DBUpdate_Client
{
    public class DBUpdateConfigurationBuilder
    {
        private string workingDirectory;
        private string xsdName;
        private bool fileLogger;
        private bool consoleLogger;

        public DBUpdateConfigurationBuilder()
        {
            Reset();
        }

        public DBUpdateConfigurationBuilder Reset()
        {
            this.workingDirectory = null;
            this.xsdName = null;
            this.fileLogger = false;
            this.consoleLogger = false;
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
        public DBUpdateConfigurationBuilder SetFileLogger(bool value)
        {
            this.fileLogger = value;

            return this;
        }
        public DBUpdateConfigurationBuilder SetConsoleLogger(bool value)
        {
            this.consoleLogger = value;

            return this;
        }

        public DBUpdateConfiguration Build() => new DBUpdateConfiguration(workingDirectory, xsdName, fileLogger, consoleLogger);
    }
}
