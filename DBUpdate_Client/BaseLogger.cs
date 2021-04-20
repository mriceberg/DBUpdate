namespace DBUpdate_Client
{
    public abstract class BaseLogger : ILogger
    {
        public void LogMessage(string message) => DoLogMessage(message);
        //public void LogFile(string message) => DoLogInFileMessage(message);
        
        protected abstract void DoLogMessage(string message);
        //protected abstract void DoLogInFileMessage(string message);

    }
}
