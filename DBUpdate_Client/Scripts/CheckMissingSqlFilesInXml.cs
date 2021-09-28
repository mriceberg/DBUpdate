using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class CheckMissingSqlFilesInXml
    {
        private readonly IConfigurationProvider configurationProvider;
        private DBUpdateConfiguration configuration;
        private DBUpdateParameters parameters;
        private List<string> resultAllSqlFilesNotReferencedInXmlFiles;

        public CheckMissingSqlFilesInXml(IConfigurationProvider configuration, DBUpdateParameters parameters)
        {
            this.configurationProvider = configuration;
            this.configuration = ReadConfiguration();
            this.parameters = parameters;
        }

        public IEnumerable<string> Scan()
        {
            string fileFolder = configuration.WorkingDirectory;
            string file = configuration.XsdName;
            string filePath = fileFolder + "\\" + file;

            var sqlFiles = ScanSQLFiles(fileFolder);
            var sqlScriptInXml = GetAllScriptsInAllXml(GetXmlFiles(), filePath);
            resultAllSqlFilesNotReferencedInXmlFiles = new List<string>();


            foreach (var sqlFile in sqlFiles)
            {
                if (!sqlScriptInXml.Contains(sqlFile)) {
                    resultAllSqlFilesNotReferencedInXmlFiles.Add(sqlFile);
                    Console.WriteLine(sqlFile);
                }
            }
            return resultAllSqlFilesNotReferencedInXmlFiles;
        }

        private IEnumerable<string> GetAllScriptsInAllXml(IEnumerable<string> xmlFiles, string filePath)
        {
            string fileFolder = Path.GetDirectoryName(filePath);
            List<string> listScripts = new List<string>();

            foreach (var xmlfile in xmlFiles)
            {
                XDocument descriptor = XDocument.Load(xmlfile);

                foreach (var blockDefinition in descriptor.Root.Element("blockDefinitions").Elements("blockDefinition"))
                {
                    listScripts.AddRange(blockDefinition.Elements("script").Select(s => s.Value.ToLower()));

                }
            }
            return listScripts;
        }

        private IEnumerable<string> ScanSQLFiles(string fileFolder)
        {
            List<string> sqlFiles;
            sqlFiles = Directory.GetFiles(fileFolder, "*.sql").Select(file => Path.GetFileName(file).ToLower()).ToList();
            return sqlFiles;
        }

        private IEnumerable<string> GetXmlFiles()
        {
            return new DBUpdateExecutionDescriptorProvider().GetFilesToRead(this.configuration.WorkingDirectory);
        }

        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this.configurationProvider).Read();
    }
}
