using System;
using System.IO;

namespace DBUpdate_Client
{
    public class ConsoleLogger : ILogger
    {
        //protected override void DoLogMessage(string message) => Console.WriteLine(message);
        //protected override void DoLogInFileMessage(string message) => WriteLogFile(message);


        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

            //private void WriteLogFile(string strMessage)
            //{
            //    try
            //    {
            //        FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", "C:\\temp\\workingdir", "log.txt"), FileMode.Append, FileAccess.Write);
            //        StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
            //        objStreamWriter.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + " - " + strMessage);
            //        objStreamWriter.Close();
            //        objFilestream.Close();

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}

        }
}
