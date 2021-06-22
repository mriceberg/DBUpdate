using Xunit;
using DBUpdate_Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBUpdate_Unit_Test.Tests
{
    public class MultiCastLoggerTests
    {
        [Fact]
        public void Log_ValidLoggers_OK()
        {
            //Arrange
            SpyLoggerTest spyLogger1 = new SpyLoggerTest();
            SpyLoggerTest spyLogger2 = new SpyLoggerTest();
            SpyLoggerTest spyLogger3 = new SpyLoggerTest();

            MultiCastLogger actualLogger = new MultiCastLogger(spyLogger1, spyLogger2, spyLogger3);


            //ACT
            actualLogger.LogMessage("testasdasdasd");
            bool actualSpyCalled = spyLogger1.Called && spyLogger2.Called && spyLogger3.Called;

            //Assert
            Assert.True(actualSpyCalled);
        }

        [Fact]
        public void LogMessage_NullLogger_Ok()
        {
            //Arrange
            SpyLoggerTest spyLogger1 = new SpyLoggerTest();
            SpyLoggerTest spyLogger2 = null;
            SpyLoggerTest spyLogger3 = new SpyLoggerTest();

            MultiCastLogger actualLogger = new MultiCastLogger(spyLogger1, spyLogger2, spyLogger3);


            //Act
            actualLogger.LogMessage("testasdasdasd");
            bool actualSpyCalled = true;

            //Assert
            Assert.True(actualSpyCalled);
        }


    }
}