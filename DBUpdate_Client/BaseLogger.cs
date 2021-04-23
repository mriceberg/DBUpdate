namespace DBUpdate_Client
{
    public abstract class BaseLogger : ILogger
    {
        public void LogMessage(string message) => DoLogMessage(message);
        protected abstract void DoLogMessage(string message);

    }
}
