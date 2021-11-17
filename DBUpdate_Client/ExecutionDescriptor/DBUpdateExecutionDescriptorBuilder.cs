using System.Collections.Generic;
using System.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorBuilder
    {
        private string path;
        private string connectionStringName;
        private string name;
        private IList<DBUpdateExecutionBlockDescriptor> blocks;
        private IList<DBUpdateExecutionBlockDescriptor> blocksToExecute;
        //private List<string> missingSQLFileInXml;

        public DBUpdateExecutionDescriptorBuilder()
        {
            Reset();
        }

        public DBUpdateExecutionDescriptorBuilder Reset()
        {
            this.path = null;
            this.connectionStringName = null;
            this.name = null;
            this.blocks = new List<DBUpdateExecutionBlockDescriptor>();
            this.blocksToExecute = new List<DBUpdateExecutionBlockDescriptor>();
            //this.missingSQLFileInXml = new List<string>();

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
        public DBUpdateExecutionDescriptorBuilder SetName(string value)
        {
            this.name = value;

            return this;
        }
        public DBUpdateExecutionDescriptorBuilder AddBlock(DBUpdateExecutionBlockDescriptor value)
        {
            blocks.Add(value);

            return this;
        }
        public DBUpdateExecutionDescriptorBuilder AddBlockToExecute(DBUpdateExecutionBlockDescriptor value)
        {
            blocksToExecute.Add(value);

            return this;
        }
        //public DBUpdateExecutionDescriptorBuilder AddMissingSQLFileInXml(IEnumerable<string> values)
        //{
        //    missingSQLFileInXml.AddRange(values);

        //    return this;
        //}
        public DBUpdateExecutionDescriptorBuilder AddBlockToExecute(string blockName) => AddBlockToExecute(blocks.Single(b => b.Name == blockName));

        public DBUpdateExecutionDescriptor Build()
            => new DBUpdateExecutionDescriptor(path, connectionStringName, blocks, blocksToExecute, name);
    }
}
