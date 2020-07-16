namespace DBUpdate_Client
{
    public interface ConfigurationProvider
    {
        string GetAppSetting(string settingName, string valueIfNotFound = null);
        string GetConnectionString(string connectionStringName, string valueIfNotFound = null);
    }
}
