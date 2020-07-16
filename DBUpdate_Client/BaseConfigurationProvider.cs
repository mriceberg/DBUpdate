namespace DBUpdate_Client
{
    public abstract class BaseConfigurationProvider : ConfigurationProvider
    {
        public string GetAppSetting(string settingName, string valueIfNotFound = null) => DoGetAppSetting(settingName) ?? valueIfNotFound;

        protected abstract string DoGetAppSetting(string settingName);
    }
}
