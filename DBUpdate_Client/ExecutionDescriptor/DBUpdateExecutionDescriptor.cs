using System.Collections.Generic;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptor
    {
        public string Path { get; private set; }
        public string ConnectionStringName { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<DBUpdateExecutionBlockDescriptor> Blocks { get; private set; }
        public IEnumerable<DBUpdateExecutionBlockDescriptor> BlocksToExecute { get; private set; }
        public IEnumerable<string> MissingSQLFileInXml { get; private set; }

        public DBUpdateExecutionDescriptor(string path, string connectionStringName, IEnumerable<DBUpdateExecutionBlockDescriptor> blocks,
            IEnumerable<DBUpdateExecutionBlockDescriptor> blocksToExecute,string name, IEnumerable<string> missingSQLFileInXml)
        {
            this.Path = path;
            this.ConnectionStringName = connectionStringName;
            this.Name = name;
            this.Blocks = blocks;
            this.BlocksToExecute = blocksToExecute;
            this.MissingSQLFileInXml = missingSQLFileInXml;
        }
    }
}
