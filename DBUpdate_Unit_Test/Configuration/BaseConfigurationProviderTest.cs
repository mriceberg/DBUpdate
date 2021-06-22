using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DBUpdate_Unit_Test.Configuration
{
    public class BaseConfigurationProviderTest
    {
        [Fact]
        //[InlineData(true, false, true, false)]
        public void GetAppSetting_DoGetAppSetting_OK()
        {
            // Arrage

            // Act

            // Assert

        }

        [Theory]
        [InlineData("testc","test")]
        public void GetConnectionString_DoGetConnectionString_OK(string settingName, string valueIfNotFound)
        {
            // Arrage

            // Act

            // Assert

        }
    }
}
