namespace DBUpdate_Client
{
    public class DBUtilExecutionDescriptor
    {
        public string Path { get; private set; }

        public DBUtilExecutionDescriptor(string path)
        {
            this.Path = path;
        }
    }
}
