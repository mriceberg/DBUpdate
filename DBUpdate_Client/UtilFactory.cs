namespace DBUpdate_Client
{
    public interface UtilFactory
    {
        ConfigurationProvider MakeConfigurationProvider();
        ILogger MakeLogger();
    }
}
