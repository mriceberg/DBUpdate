using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionBlockDescriptor
    {
        public string Name { get; private set; }
        public IEnumerable<DBUpdateScript> Scripts { get; private set; }

        public DBUpdateExecutionBlockDescriptor(string name, IEnumerable<DBUpdateScript> scripts)
        {
            this.Name = name;
            this.Scripts = scripts;
        }
    }
}
