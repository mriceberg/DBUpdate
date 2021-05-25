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
        private IEnumerable<ILogger> _loggers;

        public MultiCastLogger(params ILogger[] loggers)
        {
            this._loggers = loggers.Where(lg => lg != null).ToArray();
        }

        protected override void DoLogMessage(string message)
        {
            foreach(var logger in _loggers)
            {
                logger.LogMessage(message);
            }
        }
    }
}
