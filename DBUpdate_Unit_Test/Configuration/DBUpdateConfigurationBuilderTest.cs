using DBUpdate_Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DBUpdate_Unit_Test.Configuration
{
    public class DBUpdateConfigurationBuilderTest
    {
        [Fact]
        public void Reset_SetAllNull_OK()
        {
            // Arrage

            // Act

            // Assert

        }

        [Fact]
        public void SetXsdName_SetValue_NotNull()
        {
            // Arrage
            string value = null;
            DBUpdateConfigurationBuilder dBUpdateConfigurationBuilder = new DBUpdateConfigurationBuilder();

            // Act
            var result = dBUpdateConfigurationBuilder.SetXsdName(value);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void SetWorkingDirectory_SetValue_OK()
        {
            // Arrage

            // Act

            // Assert

        }

        [Fact]
        public void SetFileLogger_SetValue_OK()
        {
            // Arrage

            // Act

            // Assert

        }

        [Fact]
        public void SetConsoleLogger_SetValue_OK()
        {
            // Arrage

            // Act

            // Assert

        }
    }
}
