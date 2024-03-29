﻿using System;
using System.Collections;
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
        private readonly ILogger _logger;
        private readonly DBUpdateParameters _param;
        private readonly DBUpdateConfiguration _config;
        private readonly IConnectionProvider _connectionProvider;
        private readonly DBUpdateExecutionDescriptor _executionDescriptor;
        private XmlDocument _document;

        public DBUpdateCheck(ILogger logger, DBUpdateParameters param, IConfigurationProvider configurationProvider, DBUpdateExecutionDescriptor executionDescriptor)
        {
            _logger = logger;
            _param = param;
            _config = new DBUpdateConfigurationReader(configurationProvider).Read();
            _executionDescriptor = executionDescriptor;

            _connectionProvider = new ConstantConnectionProvider(configurationProvider.GetConnectionString(_executionDescriptor.ConnectionStringName));

            if (_logger == null) throw new ArgumentNullException(nameof(logger));
        }

        public void StartTest()
        {
            if (_param.IsTest)
            {
                Log("Starting all the test check ...");

                //Vérifie les tables et les recrées si elles ont été drop
                CheckDBStructure(_connectionProvider);
                TestIsTest();
            }
        }

        private void TestIsTest()
        {
            foreach (var file in new DBUpdateExecutionDescriptorProvider().GetFilesToRead(_config.WorkingDirectory))
            {
                CheckParameters(_param);
                CheckXML(file);
                CheckSqlRefInBlock(file);
                CheckBatchGoCommentedInMultiLineComment(file);
                if (_param.IsSimulation)
                {
                    StatsWithSimulationMode(_connectionProvider, file);
                }
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
        private void CheckParameters(DBUpdateParameters _param)
        {
            if(!String.IsNullOrEmpty(_param.IsUpToBlock) && !String.IsNullOrEmpty(_param.IsBlockName))
            {
                _logger.LogMessage("Attention 2 paramètres sont en conflits ! MaxBlockName et BlockName. Seulement BlockName sera prit en compte.");
            }
        }
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Log("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Log("\tValidation error: " + args.Message);

        }
        private void CheckSqlRefInBlock(string filePath)
        {
            DBUpdateExecutionDescriptorReader dBUpdateExecutionDescriptorReader = new DBUpdateExecutionDescriptorReader();
            var descriptor = dBUpdateExecutionDescriptorReader.Read(filePath);

            // For each block not run yet
            foreach (var block in descriptor.BlocksToExecute)
            {
                Log($"Checking block {block.Name}");

                // For each script to execute
                foreach (var script in block.Scripts)
                //foreach (var script in descriptor.Blocks.ToList(aa))
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
        private void CheckBatchGoInLastLine(IEnumerable<IEnumerable<string>> batches)
        {
            if (batches.Last().Last().StartsWith("GO") || batches.Last().ElementAt(batches.Count() - 1).StartsWith("GO"))
            {
                Log("Error: There is a GO in the last line of the file");
            }
        }
        private void StatsWithSimulationMode(IConnectionProvider connectionProvider,string file)
        {
            
            IEnumerable<DBUpdateExecutionDescriptor> executionDescriptors = new DBUpdateExecutionDescriptorReader()
                .ReadAll(new DBUpdateExecutionDescriptorProvider()
                .GetFilesToRead(this._config.WorkingDirectory));

            ScriptGateway scriptGateway = new ScriptGateway(connectionProvider);

            var nbrOfBlock = 0;
            var nbrOfScriptsByBlock = 0;
            int nbrOfBatch = 0;
            foreach (DBUpdateExecutionDescriptor descriptor in executionDescriptors)
            {
                var batches = new DBUpdateFileScriptToBatch().GetScriptAndSplit(file);
                var executedBlockNames = scriptGateway.GetExecutedScriptNames().Select(sn => descriptor.BlocksToExecute.FirstOrDefault(bte => bte.Name == sn)).Where(b => b != null);
                
                nbrOfBlock += descriptor.BlocksToExecute.Except(executedBlockNames).Count();
                Log($"There are {nbrOfBlock} blocks to execute");

                foreach (var block in descriptor.Blocks) 
                {
                    nbrOfScriptsByBlock += block.Scripts.Count();
                    Log($"There are {nbrOfScriptsByBlock} scripts in block {block.Name}");

                    foreach (IEnumerable<String> batch in batches)
                    {
                        nbrOfBatch++;
                    }

                    Log($"There are {nbrOfBatch} batch in scripts\n");
                }             
            }
        }
        private void CheckDBStructure(IConnectionProvider connectionProvider) => new DBUpdateStructureValidator(connectionProvider).EnsureStructureExists();
        private void Log(string message) => this._logger?.LogMessage(message);
    }
}
