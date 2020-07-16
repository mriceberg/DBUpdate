namespace DBUpdate_Client
{
    public class DBUtilExecutionDescriptor
    {
        public string Path { get; private set; }
        public string ConnectionStringName { get; private set; }

        public DBUtilExecutionDescriptor(string path, string connectionStringName)
        {
            this.Path = path;
            this.ConnectionStringName = connectionStringName;
        }
    }
}
