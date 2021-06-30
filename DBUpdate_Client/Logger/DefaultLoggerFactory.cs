using DBUpdate_Client.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DBUpdate_Client
{
    public class DefaultLoggerFactory : ILoggerFactory
    {
        public ILogger MakeConsoleLogger() => new TextWriterLogger(Console.Out);
        public ILogger MakeFileLogger(StreamWriter file) => new TextWriterLogger(file);
        public ILogger MakeMultiCastLogger(params ILogger[] loggers) => new MultiCastLogger(loggers);
    }
}
