namespace DBUpdate_Client
{
    public class DBUpdateConfiguration
    {
        public string WorkingDirectory { get; private set; }
        public string XsdName { get; private set; }

        public DBUpdateConfiguration(string workingDirectory, string xsdName)
        {
            this.WorkingDirectory = workingDirectory;
            this.XsdName = xsdName;
        }
    }
}
