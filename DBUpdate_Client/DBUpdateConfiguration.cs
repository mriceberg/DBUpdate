namespace DBUpdate_Client
{
    public class DBUpdateConfiguration
    {
        public string WorkingDirectory { get; private set; }

        public DBUpdateConfiguration(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
        }
    }
}
