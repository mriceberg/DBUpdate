using System;
using System.Collections.Generic;
using System.Text;
using DBUpdate_Client;
using Xunit;

namespace DBUpdate_Unit_Test.Logger
{
    public abstract class BaseLoggerUnitTestsFixure 
    {
        public abstract void LogMessage(string message);

        [Fact]
        public void LogMessage_NotNull_OK()
        {
            //Arrange
            BaseLoggerTests baseLoggerTests = new BaseLoggerTests();

            // Act
            baseLoggerTests.LogMessage("");
            
            // Assert
            
        }

        ////public bool Called { get; private set; }


        ////public void LogMessage(string expectedMessage)
        ////{
        ////    Called = true;
        ////}
    }
}
