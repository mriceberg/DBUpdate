using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorReader
    {
        public IEnumerable<DBUpdateExecutionDescriptor> ReadAll(IEnumerable<string> files)
            => files.Select(file => Read(file));
        public DBUpdateExecutionDescriptor Read(string filePath)
        {
            string fileFolder = Path.GetDirectoryName(filePath);
            XDocument descriptor = XDocument.Load(filePath);
            string connectionStringName = descriptor.Root.Element("configuration").Element("connectionStringName").Value;
            
            DBUpdateExecutionDescriptorBuilder builder = new DBUpdateExecutionDescriptorBuilder()
                .SetPath(filePath)
                .SetConnectionStringName(connectionStringName);

            DBUpdateExecutionBlockDescriptorBuilder blockBuilder = new DBUpdateExecutionBlockDescriptorBuilder();
            DBUpdateScriptBuilder scriptBuilder = new DBUpdateScriptBuilder();
            foreach (var blockElement in descriptor.Root.Element("blockDefinitions").Elements("blockDefinition"))
            {
                blockBuilder.Reset();
                string blockName = blockElement.Attribute("name").Value;
                blockBuilder.SetName(blockName);

                foreach(var scriptElement in blockElement.Elements("script"))
                {
                    string scriptName = scriptElement.Value;
                    scriptBuilder.Reset();
                    scriptBuilder.SetName(scriptName);
                    scriptBuilder.SetPath(fileFolder);
                    var script = scriptBuilder.Build();
                    blockBuilder.AddScript(script);
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
    }
}
