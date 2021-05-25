using System.Configuration;
using System.Linq;

namespace DBUpdate_Client
{
    public class DefaultConfigurationProvider : BaseConfigurationProvider
    {
        protected override string DoGetAppSetting(string settingName) => ConfigurationManager.AppSettings[settingName];
        protected override string DoGetConnectionString(string connectionStringName) => ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
    }
}
