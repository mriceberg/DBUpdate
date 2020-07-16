namespace DBUpdate_Client
{
    public class DefaultUtilFactory : UtilFactory
    {
        public ConfigurationProvider MakeConfigurationProvider() => new DefaultConfigurationProvider();
        public Logger MakeLogger() => new ConsoleLogger();
    }
}
