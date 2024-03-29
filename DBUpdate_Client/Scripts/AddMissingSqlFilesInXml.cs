﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DBUpdate_Client
{
    public class AddMissingSqlFilesInXml
    {
        private readonly ILogger _logger;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly DBUpdateConfiguration _configuration;
        private readonly DBUpdateParameters _parameters;
        private IEnumerable<string> _listOfFilesToAdd;
        private string _xmlFileFromScanParameters;
        private XElement _blockDefinitionDummy;
        private string _nameDummyBlockByParameters;

        public AddMissingSqlFilesInXml(ILogger logger, IConfigurationProvider configuration, DBUpdateParameters parameters)
        {
            this._logger = logger;
            this._configurationProvider = configuration;
            this._configuration = ReadConfiguration();
            this._parameters = parameters;
            this._listOfFilesToAdd = GetListOfFilesToAdd();
            this._xmlFileFromScanParameters = SetXMlFileName();
            this._nameDummyBlockByParameters = parameters.NameOfDummyBlock;
        }
        public void AddMissingScriptsInXml()
        {
            if (_listOfFilesToAdd.Any())
            {
                XDocument descriptor = XDocument.Load(_xmlFileFromScanParameters);
                XAttribute dummyBlock = new XAttribute("name", _nameDummyBlockByParameters);

                if ((_blockDefinitionDummy = GetAllXAttributesNameValue(descriptor)) == null)
                {
                    descriptor.Root.Elements("blockDefinitions").Last().Add(_blockDefinitionDummy = new XElement("blockDefinition", dummyBlock));
                    Log("Création d'un nouveau dummyBlock");
                }

                foreach (string script in _listOfFilesToAdd)
                {
                    _blockDefinitionDummy.Add(new XElement("script", script));
                }

                descriptor.Save(_xmlFileFromScanParameters);
            }
        }

        //private bool CheckIfOnlyOneDummyBlock()
        //{
        //    XmlDocument descriptor = new XmlDocument();
        //    descriptor.Load(_xmlFileFromScanParameters);
        //    XmlNodeList elt2 = descriptor.SelectNodes("//blockDefinition[@name='DummyBlock']") as XmlNodeList;
        //    if (elt2.Count > 1)
        //    {
        //        Log("Attention vous avez plusieurs blockDefinition avec le nom DummyBlock.");
        //        return false;
        //    }
        //    return true;
        //}

        private XElement GetAllXAttributesNameValue(XDocument descriptor)
            => descriptor.Descendants("blockDefinition").Where(node => node.Attribute("name").Value == _nameDummyBlockByParameters).SingleOrDefault();

        private IEnumerable<string> GetListOfFilesToAdd()
        {
            CheckMissingSqlFilesInXml checkMissingSqlFiles = new CheckMissingSqlFilesInXml(_configurationProvider, _parameters);
            return _listOfFilesToAdd = checkMissingSqlFiles.Scan();
        }

        private string SetXMlFileName()
        {
            return _xmlFileFromScanParameters = _configuration.WorkingDirectory + "\\" + _parameters.IsScan + ".xml";
        }

        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this._configurationProvider).Read();

        private void Log(string message) => this._logger?.LogMessage(message);
    }
}
