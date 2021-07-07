using Xunit;
using DBUpdate_Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DBUpdate_Client.Logger;
using DBUpdate_Unit_Test.Logger;

namespace DBUpdate_Unit_Test.Tests
{
    public class TextWriterLoggerTests
    {
        [Fact]
        public void LogMessage_ValidMessage_OK()
        {

            // Arrage
            var expectedMessage = "blalblabla test";
            var sb = new StringBuilder();
            string actualMessage;

            // Act
            using (var writer = new StringWriter(sb))
            {
                var actualWriter = new TextWriterLogger(writer);
                actualWriter.LogMessage(expectedMessage);

                actualMessage = sb.ToString();
            }

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void LogMessage_TextWriterNotNull_OK()
        {

            // Arrage
            TextWriterLogger textWriterLogger = new TextWriterLogger(Console.Out);

            // Act
            textWriterLogger.LogMessage("measdas");
            
            // Assert
            Assert.False(textWriterLogger.IsNullOrWhiteSpace(), "TextWriterLogger is null or whitespace !");
        }
    }
}