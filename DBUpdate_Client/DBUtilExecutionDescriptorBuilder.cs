namespace DBUpdate_Client
{
    public class DBUtilExecutionDescriptorBuilder
    {
        private string path;

        public DBUtilExecutionDescriptorBuilder()
        {
            Reset();
        }

        public DBUtilExecutionDescriptorBuilder Reset()
        {
            this.path = null;

            return this;
        }

        public DBUtilExecutionDescriptorBuilder SetPath(string value)
        {
            this.path = value;

            return this;
        }

        public DBUtilExecutionDescriptor Build()
            => new DBUtilExecutionDescriptor(path);
    }
}
