namespace DBUpdate_Client
{
    public class DBUtilExecutionDescriptorBuilder
    {
        private string path;
        private string connectionStringName;

        public DBUtilExecutionDescriptorBuilder()
        {
            Reset();
        }

        public DBUtilExecutionDescriptorBuilder Reset()
        {
            this.path = null;
            this.connectionStringName = null;

            return this;
        }

        public DBUtilExecutionDescriptorBuilder SetPath(string value)
        {
            this.path = value;

            return this;
        }
        public DBUtilExecutionDescriptorBuilder SetConnectionStringName(string value)
        {
            this.connectionStringName = value;

            return this;
        }

        public DBUtilExecutionDescriptor Build()
            => new DBUtilExecutionDescriptor(path, connectionStringName);
    }
}
