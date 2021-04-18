using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class FileLogger: ILogger
    {

        public void LogMessage(string message)
        {
            try
            {
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", "C:\\temp\\workingdir", "log.txt"), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + " - " + message);
                objStreamWriter.Close();
                objFilestream.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //protected override void DoLogInFileMessage(string message)
        //{
            
        //}
        //protected override void DoLogMessage(string message)
        //{
            
        //}
    }
}
