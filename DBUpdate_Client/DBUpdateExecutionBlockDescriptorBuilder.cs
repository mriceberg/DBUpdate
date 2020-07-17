using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionBlockDescriptorBuilder
    {
        private string name;
        private IList<string> scriptNames;

        public DBUpdateExecutionBlockDescriptorBuilder()
        {
            Reset();
        }

        public DBUpdateExecutionBlockDescriptorBuilder Reset()
        {
            name = null;
            scriptNames = new List<string>();

            return this;
        }

        public DBUpdateExecutionBlockDescriptorBuilder SetName(string value)
        {
            name = value;

            return this;
        }
        public DBUpdateExecutionBlockDescriptorBuilder AddScriptName(string value)
        {
            scriptNames.Add(value);

            return this;
        }

        public DBUpdateExecutionBlockDescriptor Build()
            => new DBUpdateExecutionBlockDescriptor(name, scriptNames);
    }
}
