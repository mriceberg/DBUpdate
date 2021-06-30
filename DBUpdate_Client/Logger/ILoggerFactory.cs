using System.IO;

namespace DBUpdate_Client
{
    public interface ILoggerFactory
    {
        ILogger MakeConsoleLogger();
        ILogger MakeFileLogger(StreamWriter file);
        ILogger MakeMultiCastLogger(params ILogger[] loggers);
    }
}
