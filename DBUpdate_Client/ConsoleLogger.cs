using System;
using System.IO;

namespace DBUpdate_Client
{
    public class ConsoleLogger : BaseLogger
    {
        //protected override void DoLogMessage(string message) => Console.WriteLine(message);
        //protected override void DoLogInFileMessage(string message) => WriteLogFile(message);

        //public void LogMessage(string message)
        //{
        //    Console.WriteLine(message);
        //}

        protected override void DoLogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
