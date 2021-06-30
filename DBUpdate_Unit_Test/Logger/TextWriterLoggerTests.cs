using Xunit;
using DBUpdate_Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DBUpdate_Client.Logger;

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
    }
}