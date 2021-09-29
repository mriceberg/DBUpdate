using System;
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
        private DBUpdateConfiguration _configuration;
        private DBUpdateParameters _parameters;
        private IEnumerable<string> _listOfFilesToAdd;
        private string _xmlFileFromScanParameters;
        private XElement _blockDefinitionDummy;
        string name;
        public AddMissingSqlFilesInXml(ILogger logger, IConfigurationProvider configuration, DBUpdateParameters parameters)
        {
            this._logger = logger;
            this._configurationProvider = configuration;
            this._configuration = ReadConfiguration();
            this._parameters = parameters;
            this._listOfFilesToAdd = GetListOfFilesToAdd();
            this._xmlFileFromScanParameters = SetXMlFileName();
        }
        public void AddMissingScriptsInXml()
        {
            if (_listOfFilesToAdd.Any())
            {
                XDocument descriptor = XDocument.Load(_xmlFileFromScanParameters);
                XAttribute dummyBlock = new XAttribute("name", "DummyBlock");

                if (String.IsNullOrEmpty(GetAllXAttributesNameValue()))
                {
                    descriptor.Root.Elements("blockDefinitions").Last().Add(new XElement("blockDefinition", dummyBlock));
                    _blockDefinitionDummy = descriptor.Root.Element("blockDefinitions").Elements("blockDefinition").Last();
                } else
                {
                    _blockDefinitionDummy = descriptor.Root.Element("blockDefinitions").Elements("blockDefinition").Last();
                }


                if (CheckIfOnlyOneDummyBlock())
                {
                    foreach (string script in _listOfFilesToAdd)
                    {
                        _blockDefinitionDummy.Add(new XElement("script", script));
                    }
                } 
                else 
                {
                    foreach (string script in _listOfFilesToAdd)
                    {
                        Log("Le script : " + script + " ne sera pas ajouté !");
                    }
                }

                descriptor.Save(_xmlFileFromScanParameters);
            }
        }
        private bool CheckIfOnlyOneDummyBlock()
        {
            XmlDocument descriptor = new XmlDocument();
            descriptor.Load(_xmlFileFromScanParameters);
            XmlNodeList elt2 = descriptor.SelectNodes("//blockDefinition[@name='DummyBlock']") as XmlNodeList;
            if (elt2.Count > 1)
            {
                Log("Attention vous avez plusieurs blockDefinition avec le nom DummyBlock.");
                return false;
            }
            return true;
        }
        private string GetAllXAttributesNameValue()
        {
            XmlDocument descriptor = new XmlDocument();
            descriptor.Load(_xmlFileFromScanParameters);
            XmlElement elt = descriptor.SelectSingleNode("//blockDefinition[@name='DummyBlock']") as XmlElement;
            
            if (elt != null)
            {
                name = elt.GetAttribute("name");
            }
            return name;
        }

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
