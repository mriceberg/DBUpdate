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

    }
}
