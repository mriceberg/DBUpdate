using System;

namespace DBUpdate_Client
{
    public class ConsoleLogger : BaseLogger
    {
        protected override void DoLogMessage(string message) => Console.WriteLine(message);
    }
}
