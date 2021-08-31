using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class DBUpdateFileScriptToBatch : DBUpdateScriptToBatch
    {
        protected override IEnumerable<string> GetScript(string script) => File.ReadAllLines(script);
        
    }
}
