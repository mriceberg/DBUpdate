using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionBlockDescriptor
    {
        public string Name { get; private set; }
        public IEnumerable<string> ScriptNames { get; private set; }

        public DBUpdateExecutionBlockDescriptor(string name, IEnumerable<string> scriptNames)
        {
            this.Name = name;
            this.ScriptNames = scriptNames;
        }
    }
}
