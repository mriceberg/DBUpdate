using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client.Logger
{
    public class TextWriterLogger : BaseLogger
    {
        private TextWriter _textWriter;

        public TextWriterLogger(TextWriter textWriter)
        {
            this._textWriter = textWriter;
        }

        protected override void DoLogMessage(string message)
        {
            _textWriter.WriteLine(message);
            _textWriter.Flush();
        }

        public bool IsNullOrWhiteSpace()
        {
            return String.IsNullOrWhiteSpace(_textWriter.ToString());
        }
    }
}
