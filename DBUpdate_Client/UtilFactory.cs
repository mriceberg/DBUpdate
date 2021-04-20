namespace DBUpdate_Client
{
    public interface IUtilFactory
    {
        IConfigurationProvider MakeConfigurationProvider();
        ILoggerFactory MakeLoggerFactory();
    }
}
