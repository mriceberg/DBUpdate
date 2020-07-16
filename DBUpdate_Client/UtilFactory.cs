namespace DBUpdate_Client
{
    public interface UtilFactory
    {
        ConfigurationProvider MakeConfigurationProvider();
        Logger MakeLogger();
    }
}
