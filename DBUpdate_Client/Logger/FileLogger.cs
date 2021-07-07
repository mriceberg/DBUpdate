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
                using (FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", "C:\\temp\\workingdir", "log.txt"), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter objStreamWriter = new StreamWriter(objFilestream))
                    {
                        objStreamWriter.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + " - " + message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
