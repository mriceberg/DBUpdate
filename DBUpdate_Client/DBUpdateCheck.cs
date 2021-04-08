using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;


namespace DBUpdate_Client
{
    public class DBUpdateCheck
    {
        private readonly DBUpdateParameters _param;
        private readonly DBUpdateConfiguration _config;

        private XmlDocument _document;

        public DBUpdateCheck(DBUpdateParameters param, ConfigurationProvider configurationProvider)
        {
            _param = param;
            _config = new DBUpdateConfigurationReader(configurationProvider).Read();
        }

        public void StartTest()
        {
            if (_param.IsTest)
            {
                TestIsTest();
            }
        }
        
        private void TestIsTest()
        {
            foreach (var file in new DBUpdateExecutionDescriptorProvider().GetFilesToRead(_config.WorkingDirectory))
            {
                CheckXML(file);
                CheckSqlRefInBlock();
            }
        }

        private void CheckXML(string filePath)
        {
            var fileFolder = Path.GetDirectoryName(filePath);
           
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, Path.Combine(fileFolder, _config.XsdName));
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.ValidationType = ValidationType.Schema;

                _document = new XmlDocument();
                _document.Load(filePath);

                // Create the XmlReader object.
                XmlReader reader = XmlReader.Create(filePath, settings);

                // Parse the file. 
                while (reader.Read());

            } catch (XmlSchemaValidationException exception) {
                Console.WriteLine("Your XML was probably bad..." + "Exception :" + exception);
            }
        }

        // Display any warnings or errors.
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);

        }
        private void CheckSqlRefInBlock()
        {
            descriptorBlockDefinition descriptorBlockDefinition = new descriptorBlockDefinition();
            string[] lgt = descriptorBlockDefinition.script;













            //foreach (string test in lgt)
            //{
            //    // test = document.Root.Element("blockDefinitions").Element("blockDefinition").Element("script").Value;
            //    Console.WriteLine(test);

            //}

            //DBUpdateScriptBuilder scriptBuilder = new DBUpdateScriptBuilder();
            //foreach (var blockElement in document.Root.Element("blockDefinitions").Elements("blockDefinition"))
            //{
            //    string blockName = blockElement.Attribute("name").Value;

            //    foreach (var scriptElement in blockElement.Elements("script"))
            //    {
            //        string scriptName = scriptElement.Value;
            //        scriptBuilder.Reset();
            //        scriptBuilder.SetName(scriptName);
            //        var script = scriptBuilder.Build();
            //    }
            //}
        }


        // REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class descriptor
        {
            private descriptorConfiguration configurationField;
            private descriptorBlockDefinition[] blockDefinitionsField;
            private string[] blocksToExecuteField;

            /// <remarks/>
            public descriptorConfiguration configuration
            {
                get
                {
                    return this.configurationField;
                }
                set
                {
                    this.configurationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("blockDefinition", IsNullable = false)]
            public descriptorBlockDefinition[] blockDefinitions
            {
                get
                {
                    return this.blockDefinitionsField;
                }
                set
                {
                    this.blockDefinitionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("block", IsNullable = false)]
            public string[] blocksToExecute
            {
                get
                {
                    return this.blocksToExecuteField;
                }
                set
                {
                    this.blocksToExecuteField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class descriptorConfiguration
        {

            private string connectionStringNameField;

            /// <remarks/>
            public string connectionStringName
            {
                get
                {
                    return this.connectionStringNameField;
                }
                set
                {
                    this.connectionStringNameField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class descriptorBlockDefinition
        {

            private string[] scriptField;
            private string nameField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("script")]
            public string[] script
            {
                get
                {
                    return this.scriptField;
                }
                set
                {
                    this.scriptField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

    }
}
