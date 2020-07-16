namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptor
    {
        public string Path { get; private set; }
        public string ConnectionStringName { get; private set; }

        public DBUpdateExecutionDescriptor(string path, string connectionStringName)
        {
            this.Path = path;
            this.ConnectionStringName = connectionStringName;
        }
    }
}
