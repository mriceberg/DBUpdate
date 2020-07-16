namespace DBUpdate_Client
{
    public class DBUtilConfiguration
    {
        public string WorkingDirectory { get; private set; }

        public DBUtilConfiguration(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
        }
    }
}
