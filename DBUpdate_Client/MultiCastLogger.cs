using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class MultiCastLogger : BaseLogger
    {
        //private FileLogger _fileLogger;
        //private ConsoleLogger _consoleLogger;

        private IEnumerable<ILogger> _loggers;

        public MultiCastLogger(params ILogger[] loggers)
        {
            this._loggers = loggers.Where(lg => lg != null).ToArray();
            //this._fileLogger = new FileLogger();
            //this._consoleLogger = new ConsoleLogger();
        }

        protected override void DoLogMessage(string message)
        {
            foreach(var logger in _loggers)
            {
                logger.LogMessage(message);
            }
        }

        //protected override void DoLogInFileMessage(string message)
        //{
        //    _fileLogger.LogMessage(message);
        //}

        //protected override void DoLogMessage(string message)
        //{
        //    _consoleLogger.LogMessage(message);
        //}
    }
}
