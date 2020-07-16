using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class DBUtilExecutionDescriptorReader
    {
        public IEnumerable<DBUtilExecutionDescriptor> ReadAll(string folder)
            => GetFilesToRead(folder).Select(file => Read(Path.Combine(folder, file)));
        public DBUtilExecutionDescriptor Read(string filePath)
        {
            XDocument descriptor = XDocument.Load(filePath);
            string connectionStringName = descriptor.Root.Element("configuration").Element("connectionStringName").Value;
            
            DBUtilExecutionDescriptorBuilder builder = new DBUtilExecutionDescriptorBuilder()
                .SetPath(filePath)
                .SetConnectionStringName(connectionStringName);

            return builder.Build();
        }

        private IEnumerable<string> GetFilesToRead(string folder) => Directory.EnumerateFiles(folder, "Scripts*.xml");
    }
}
