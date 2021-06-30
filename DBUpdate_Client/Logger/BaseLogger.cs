using System;
using System.IO;

namespace DBUpdate_Client
{
    public abstract class BaseLogger : ILogger
    {
        public void LogMessage(string message)
        { 
            if (!String.IsNullOrWhiteSpace(message)) DoLogMessage(message);
        }
        protected abstract void DoLogMessage(string message);

    }
}
