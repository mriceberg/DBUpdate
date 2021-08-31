﻿using System;
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
            var sqlFiles = ScanSQLFiles(fileFolder);
            var sqlScriptInXml = GetAllScriptsInAllXml(GetXmlFiles(), filePath);

            return sqlScriptInXml.Except(sqlFiles);
        }


        private IEnumerable<string> GetAllScriptsInAllXml(IEnumerable<string> xmlFiles, string filePath)
        {
            string fileFolder = Path.GetDirectoryName(filePath);
            //string connectionStringName = descriptor.Root.Element("configuration").Element("connectionStringName").Value;
            IEnumerable<string> listScripts = new string[] { };

            foreach (var xmlfile in xmlFiles)
            {
                XDocument descriptor = XDocument.Load(xmlfile);
                //string connectionStringName = descriptor.Root.Element("configuration").Element("connectionStringName").Value;

                foreach (var blockDefinition in descriptor.Root.Element("blockDefinitions").Elements("blockDefinition"))
                {
                    foreach (var scriptElement in blockDefinition.Elements("script"))
                    {
                        string scriptName = scriptElement.Value;
                        listScripts.Append(scriptName);
                    }
                }
            }
            return listScripts;
        }


        private IEnumerable<string> ScanSQLFiles(string fileFolder)
        {
            IEnumerable<string> SqlFiles;
            SqlFiles = Directory.GetFiles(fileFolder, "*.sql");

            return SqlFiles;
        }
        private IEnumerable<string> GetXmlFiles()
        {
            return new DBUpdateExecutionDescriptorProvider().GetFilesToRead(this.configuration.WorkingDirectory);
        }
        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this.configurationProvider).Read();
    }
}