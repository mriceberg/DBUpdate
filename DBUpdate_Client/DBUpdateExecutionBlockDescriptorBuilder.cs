using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionBlockDescriptorBuilder
    {
        private string name;
        private IList<DBUpdateScript> scripts;

        public DBUpdateExecutionBlockDescriptorBuilder()
        {
            Reset();
        }

        public DBUpdateExecutionBlockDescriptorBuilder Reset()
        {
            name = null;
            scripts = new List<DBUpdateScript>();

            return this;
        }

        public DBUpdateExecutionBlockDescriptorBuilder SetName(string value)
        {
            name = value;

            return this;
        }
        public DBUpdateExecutionBlockDescriptorBuilder AddScript(DBUpdateScript value)
        {
            scripts.Add(value);

            return this;
        }

        public DBUpdateExecutionBlockDescriptor Build()
            => new DBUpdateExecutionBlockDescriptor(name, scripts);
    }
}
