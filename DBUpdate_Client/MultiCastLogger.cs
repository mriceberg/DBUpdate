using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class MultiCastLogger : BaseLogger
    {
        private FileLogger _fileLogger;
        private ConsoleLogger _consoleLogger;

        //private List<ILogger> _loggers;

        public MultiCastLogger(FileLogger fileLogger, ConsoleLogger consoleLogger)
        {
            this._fileLogger = fileLogger;
            this._consoleLogger = consoleLogger;
        }

        protected override void DoLogInFileMessage(string message)
        {
            _fileLogger.LogMessage(message);
        }

        protected override void DoLogMessage(string message)
        {
            _consoleLogger.LogMessage(message);
        }
    }
}
