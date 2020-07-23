using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorReader
    {
        public IEnumerable<DBUpdateExecutionDescriptor> ReadAll(string folder)
            => GetFilesToRead(folder).Select(file => Read(folder, file));
        public DBUpdateExecutionDescriptor Read(string fileFolder, string fileName)
        {
            var filePath = Path.Combine(fileFolder, fileName);
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
                string blockName = blockElement.Attribute("name");
                blockBuilder.SetName(name);

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

        private IEnumerable<string> GetFilesToRead(string folder) => Directory.EnumerateFiles(folder, "Scripts*.xml");
    }
}
