using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorReader
    {
        public IEnumerable<DBUpdateExecutionDescriptor> ReadAll(string folder)
            => GetFilesToRead(folder).Select(file => Read(Path.Combine(folder, file)));
        public DBUpdateExecutionDescriptor Read(string filePath)
        {
            XDocument descriptor = XDocument.Load(filePath);
            string connectionStringName = descriptor.Root.Element("configuration").Element("connectionStringName").Value;
            
            DBUpdateExecutionDescriptorBuilder builder = new DBUpdateExecutionDescriptorBuilder()
                .SetPath(filePath)
                .SetConnectionStringName(connectionStringName);

            DBUpdateExecutionBlockDescriptorBuilder blockBuilder = new DBUpdateExecutionBlockDescriptorBuilder();
            foreach(var blockElement in descriptor.Root.Element("blockDefinitions").Elements("blockDefinition"))
            {
                blockBuilder.Reset();

                foreach(var scriptElement in blockElement.Elements("script"))
                {
                    string script = scriptElement.Value;

                    blockBuilder.AddScriptName(script);
                }

                DBUpdateExecutionBlockDescriptor block = blockBuilder.Build();
                builder.AddBlock(block);
            }
            foreach(var blockToExecuteElement in descriptor.Root.Element("blocksToExecute").Elements("block"))
            {
                string blockName = blockToExecuteElement.Value;

                builder.AddBlockToExecute(blockName);
            }

            return builder.Build();
        }

        private IEnumerable<string> GetFilesToRead(string folder) => Directory.EnumerateFiles(folder, "Scripts*.xml");
    }
}
