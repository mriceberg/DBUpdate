namespace DBUpdate_Client
{
    public abstract class BaseLogger : Logger
    {
        public void LogMessage(string message) => DoLogMessage(message);

        protected abstract void DoLogMessage(string message);
    }
}
