using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class DBUpdateExecutionDescriptorProvider
    {
        private const string SearchPattern = "Scripts*.xml";
        public IEnumerable<string> GetFilesToRead(string folder) => Directory.EnumerateFiles(folder, SearchPattern);
    }
}
