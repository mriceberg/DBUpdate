using System;
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
        private readonly Logger _logger;
        private readonly DBUpdateParameters _param;
        private readonly DBUpdateConfiguration _config;

        private XmlDocument _document;

        public DBUpdateCheck(Logger logger, DBUpdateParameters param, ConfigurationProvider configurationProvider)
        {
            if (_logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
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
                CheckSqlRefInBlock(file);
                CheckBatchGoCommentedInMultiLineComment();
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
        private void CheckBatchGoCommentedInMultiLineComment(IEnumerable<string> scriptText)
        {
            var result = new List<IEnumerable<string>>();
            IList<string> currentBatch = new List<string>();

            bool add = true;

            foreach (string line in scriptText)
            {
                bool end = false;

                if (line.StartsWith("/*"))
                {
                    add = false;
                }
                if (line.StartsWith("*/"))
                {
                    add = true;
                    end = true;
                }

                if (add && !end)
                {
                    if (line == "GO")
                    {
                        result.Add(currentBatch);
                        currentBatch = new List<string>();
                    }
                    else
                    {
                        if (CheckBatchIsValid(line))
                        {
                            currentBatch.Add(line);
                        }
                    }
                }
            }

            if (currentBatch.Count > 0)
            {
                result.Add(currentBatch);
            }
            //return result;
        }

        // TODO: When GO is the last line of the file, execution crashes.
        private void checkBatchGoInLastLine()
        {

        }
        private bool CheckBatchIsValid(string line) => !String.IsNullOrWhiteSpace(line);
        private void Log(string message) => this._logger?.LogMessage(message);
    }
}
