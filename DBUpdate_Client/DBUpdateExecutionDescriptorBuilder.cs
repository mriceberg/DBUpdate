namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorBuilder
    {
        private string path;
        private string connectionStringName;

        public DBUpdateExecutionDescriptorBuilder()
        {
            Reset();
        }

        public DBUpdateExecutionDescriptorBuilder Reset()
        {
            this.path = null;
            this.connectionStringName = null;

            return this;
        }

        public DBUpdateExecutionDescriptorBuilder SetPath(string value)
        {
            this.path = value;

            return this;
        }
        public DBUpdateExecutionDescriptorBuilder SetConnectionStringName(string value)
        {
            this.connectionStringName = value;

            return this;
        }

        public DBUpdateExecutionDescriptor Build()
            => new DBUpdateExecutionDescriptor(path, connectionStringName);
    }
}
