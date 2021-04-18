namespace DBUpdate_Client
{
    public class DefaultUtilFactory : UtilFactory
    {
        public ConfigurationProvider MakeConfigurationProvider() => new DefaultConfigurationProvider();
        public ILogger MakeLogger() => new MultiCastLogger();

    }
}
