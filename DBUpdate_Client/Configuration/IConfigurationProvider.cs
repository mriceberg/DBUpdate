namespace DBUpdate_Client
{
    public interface IConfigurationProvider
    {
        string GetAppSetting(string settingName, string valueIfNotFound = null);
        string GetConnectionString(string connectionStringName, string valueIfNotFound = null);
    }
}
