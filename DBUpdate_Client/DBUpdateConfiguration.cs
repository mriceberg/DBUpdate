namespace DBUpdate_Client
{
    public class DBUpdateConfiguration
    {
        public string WorkingDirectory { get; private set; }
        public string XsdName { get; private set; }

        public bool FileLogger { get; private set; }
        public bool ConsoleLogger { get; private set; }

        public DBUpdateConfiguration(string workingDirectory, string xsdName, bool fileLogger, bool consoleLogger)
        {
            this.WorkingDirectory = workingDirectory;
            this.XsdName = xsdName;
            this.FileLogger = fileLogger;
            this.ConsoleLogger = consoleLogger;
        }
    }
}
