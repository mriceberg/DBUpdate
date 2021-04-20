using System.Collections.Generic;
using System.Linq;

namespace DBUpdate_Client
{
    public class DefaultLoggerFactory : ILoggerFactory
    {
        public ILogger MakeConsoleLogger() => new ConsoleLogger();

        public ILogger MakeFileLogger() => new FileLogger();

        public ILogger MakeMultiCastLogger(params ILogger[] loggers) => new MultiCastLogger(loggers);

        public ILogger MakeMultiCastLogger(bool logToConsole = true, bool logToFile = false)
        {
            IList<ILogger> loggers = new List<ILogger>();

            if (logToConsole)
            {
                loggers.Add(MakeConsoleLogger());
            }
            if (logToFile)
            {
                loggers.Add(MakeFileLogger());
            }

            return MakeMultiCastLogger(loggers.ToArray());
        }
    }
}
