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
        private readonly IConfigurationProvider _configurationProvider;
        private DBUpdateConfiguration _configuration;
        private DBUpdateParameters _parameters;
        private IEnumerable<string> _listOfFilesToAdd;
        private string _xmlFileFromScanParameters;
        private XElement _blockDefinitionDummy;
        string name;
        public AddMissingSqlFilesInXml(IConfigurationProvider configuration, DBUpdateParameters parameters)
        {
            this._configurationProvider = configuration;
            this._configuration = ReadConfiguration();
            this._parameters = parameters;
            this._listOfFilesToAdd = GetListOfFilesToAdd();
        }
        public void AddMissingScriptsInXml()
        {
            _xmlFileFromScanParameters = _configuration.WorkingDirectory + "\\" + _parameters.IsScan + ".xml";

            if (_listOfFilesToAdd.Any())
            {
                XDocument descriptor = XDocument.Load(_xmlFileFromScanParameters);
                XAttribute dummyBlock = new XAttribute("name", "DummyBlock");

                descriptor.Root.Elements("blockDefinitions").Last().Add(new XElement("blockDefinition"));

                _blockDefinitionDummy = descriptor.Root.Element("blockDefinitions").Elements("blockDefinition").Last();

                if (String.IsNullOrEmpty(GetAllXAttributesNameValue()))
                {
                    _blockDefinitionDummy.Add(dummyBlock);
                }

                foreach (string script in _listOfFilesToAdd)
                {
                    _blockDefinitionDummy.Add(new XElement("script", script));
                }
                descriptor.Save(_xmlFileFromScanParameters);

            }
        }

        private string GetAllXAttributesNameValue()
        {
            //XDocument descriptor = XDocument.Load(_xmlFileFromScanParameters);
            XmlDocument descriptor = new XmlDocument();
            descriptor.Load(_xmlFileFromScanParameters);
            //IEnumerable<XElement> listOfElements = descriptor.Elements();
            //List<string> blockDefinitionAttributeNameValue = new List<string>();
            //foreach (var nameValue in listOfElements)
            //{
            //    blockDefinitionAttributeNameValue.Add(nameValue.ToString());
            //}
            //return blockDefinitionAttributeNameValue;
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

        private DBUpdateConfiguration ReadConfiguration() => new DBUpdateConfigurationReader(this._configurationProvider).Read();
    }
}
