namespace DBUpdate_Client
{
    public class DefaultUtilFactory : IUtilFactory
    {
        public IConfigurationProvider MakeConfigurationProvider() => new DefaultConfigurationProvider();
        public ILoggerFactory MakeLoggerFactory() => new DefaultLoggerFactory();

    }
}
