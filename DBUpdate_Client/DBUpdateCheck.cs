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
        public bool IsTest;

        public DBUpdateCheck(bool isTest)
        {
            this.IsTest = isTest;
        }

        public void StartTest()
        {
            if (IsTest)
            {
                TestIsTest();
            }
        }

        private void TestIsTest()
        {
            CheckXML(@"c:\temp\workingdir", "ScriptsETT.xml");
            //ReadExecutionDescriptors();
        }

        private void CheckXML(string fileFolder, string fileName)
        {
            var filePath = Path.Combine(fileFolder, fileName);
           
            try
            {

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, @"c:\temp\workingdir\ScriptsETT.xsd");
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.ValidationType = ValidationType.Schema;

                // Create the XmlReader object.
                XmlReader reader = XmlReader.Create(@"c:\temp\workingdir\ScriptsETT.xml", settings);

                XmlDocument document = new XmlDocument();
                document.Load(filePath);

                //ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

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

    }
}
