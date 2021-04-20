namespace DBUpdate_Client
{
    public abstract class BaseConfigurationProvider : IConfigurationProvider
    {
        public string GetAppSetting(string settingName, string valueIfNotFound = null) => DoGetAppSetting(settingName) ?? valueIfNotFound;
        public string GetConnectionString(string connectionStringName, string valueIfNotFound = null) => DoGetConnectionString(connectionStringName) ?? valueIfNotFound;

        protected abstract string DoGetAppSetting(string settingName);
        protected abstract string DoGetConnectionString(string connectionStringName);
    }
}
