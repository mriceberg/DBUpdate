using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;


// TODO: Remplacer Console.WriteLine par _logger.LogMessage

namespace DBUpdate_Client
{
    public class DBUpdateCheck
    {
        private readonly ILogger _logger;
        private readonly DBUpdateParameters _param;
        private readonly DBUpdateConfiguration _config;

        private XmlDocument _document;

        public DBUpdateCheck(ILogger logger, DBUpdateParameters param, IConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _param = param;
            _config = new DBUpdateConfigurationReader(configurationProvider).Read();

            if (_logger == null) throw new ArgumentNullException(nameof(logger));
        }

        public void StartTest()
        {
            if (_param.IsTest)
            {
               // _logger.LogMessage("Starting all the check ...");
                _logger.LogMessage("Starting all the test check ...");
                TestIsTest();
            }
        }

        private void TestIsTest()
        {
            foreach (var file in new DBUpdateExecutionDescriptorProvider().GetFilesToRead(_config.WorkingDirectory))
            {
                CheckXML(file);
                CheckSqlRefInBlock(file);
                CheckBatchGoCommentedInMultiLineComment(file);
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
                while (reader.Read()) ;

            }
            catch (XmlSchemaValidationException exception)
            {
                Log("Your XML was probably bad..." + "Exception :" + exception);
            }
        }

        // Display any warnings or errors.
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);

        }
        private void CheckSqlRefInBlock(string filePath)
        {
            // TODO: Vérifier que les BlocksToExecute existent bien dans les Blocks
            DBUpdateExecutionDescriptorReader dBUpdateExecutionDescriptorReader = new DBUpdateExecutionDescriptorReader();
            var descriptor = dBUpdateExecutionDescriptorReader.Read(filePath);

            // For each block not run yet
            foreach (var block in descriptor.BlocksToExecute)
            {
                Log($"Checking block {block.Name}");

                // For each script to execute
                foreach (var script in block.Scripts)
                {
                    Log($"Checking script {script.Name}");
                    // Check that the script is available
                    if (!script.Exists)
                    {
                        // Fail if the script is missing
                        throw new FileNotFoundException($"Missing script in block {block}. ", script.FullPath);
                    }
                }
            }
        }

        // TODO: Pendant l'exécution des scripts SQL, ils sont divisés en lots "batches", séparés par des lignes contenant le mot GO et rien d'autre. Lorsque cette ligne se trouve dans un commentaire de plusieurs lignes, elle doit être ignorée.
        private void CheckBatchGoCommentedInMultiLineComment(string file)
        {
            var batches = new DBUpdateFileScriptToBatch().GetScriptAndSplit(file);

            foreach (IEnumerable<String> batch in batches)
            {
                int comment = 0;

                foreach (string line in batch)
                {

                    if (line.StartsWith("/*"))
                    {
                        comment++;
                    }

                    if (line.StartsWith("*/"))
                    {
                        comment--;
                    }
                }

                if (comment >= 1) { 
                    Log("Error: There is a comment opening without closing ...");
                }
                if (comment < 0 )
                {
                    Log("Error: There is a comment closing without opening ...");
                }
            }

            CheckBatchGoInLastLine(batches);
        }

        // TODO: When GO is the last line of the file, execution crashes.
        private void CheckBatchGoInLastLine(IEnumerable<IEnumerable<string>> batches)
        {
            if (batches.Last().Last().StartsWith("GO") || batches.Last().ElementAt(batches.Count() - 1).StartsWith("GO"))
            {
                Log("Error: There is a GO in the last line of the file");
            }
        }
        //private bool CheckBatchIsValid(string line) => !String.IsNullOrWhiteSpace(line);
        private void Log(string message) => this._logger?.LogMessage(message);
    }
}
