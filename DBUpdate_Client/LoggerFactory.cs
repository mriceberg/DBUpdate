namespace DBUpdate_Client
{
    public interface ILoggerFactory
    {
        ILogger MakeConsoleLogger();
        ILogger MakeFileLogger();
        ILogger MakeMultiCastLogger(params ILogger[] loggers);
        ILogger MakeMultiCastLogger(bool logToConsole = true, bool logToFile = false);
    }
}
