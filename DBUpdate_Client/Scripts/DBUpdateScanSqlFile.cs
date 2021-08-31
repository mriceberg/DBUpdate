using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class DBUpdateScanSqlFile
    {
        private readonly IConfigurationProvider configurationProvider;
        private DBUpdateConfiguration configuration;
        private DBUpdateParameters parameters;

        public DBUpdateScanSqlFile(IConfigurationProvider configuration, DBUpdateParameters parameters)
        {
            this.configurationProvider = configuration;
            this.configuration = ReadConfiguration();
            this.parameters = parameters;
        }

        public IEnumerable<string> Scan(string filePath)
        {
            string fileFolder = Path.GetDirectoryName(filePath);
            ScanSQLFiles(fileFolder);
            GetAllScriptsNotReferencedInAllXml(GetXmlFiles(), filePath);
            
        }




        private IEnumerable<string> GetAllScriptsNotReferencedInAllXml(IEnumerable<string> xmlFiles, string filePath)
        {
            string fileFolder = Path.GetDirectoryName(filePath);
            XDocument descriptor = XDocument.Load(filePath);

        }









        private IEnumerable<string> ScanSQLFiles(string fileFolder)
        {
            IEnumerable<string> missingSqlFile;
            missingSqlFile = Directory.GetFiles(fileFolder, "*.sql");

            return missingSqlFile;
        }
        private IEnumerable<string> GetXmlFiles()
        {
            return new DBUpdateExecutionDescriptorProvider().GetFilesToRead(this.configuration.WorkingDirectory);
        }
        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this.configurationProvider).Read();
    }
}
