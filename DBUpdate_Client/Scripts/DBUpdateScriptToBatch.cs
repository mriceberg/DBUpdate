using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public abstract class DBUpdateScriptToBatch
    {
        public IEnumerable<IEnumerable<string>> GetScriptAndSplit(string script) => SplitScriptIntoBatches(GetScript(script));

        protected abstract IEnumerable<string> GetScript(string script);

        private IEnumerable<IEnumerable<string>> SplitScriptIntoBatches(IEnumerable<string> scriptText)
        {
            var result = new List<IEnumerable<string>>();
            IList<string> currentBatch = new List<string>();

            foreach (string line in scriptText)
            {
                if (line == "GO")
                {
                    result.Add(currentBatch);
                    currentBatch = new List<string>();
                }
                else
                {
                    currentBatch.Add(line);
                }
            }

            if (currentBatch.Count > 0)
            {
                result.Add(currentBatch);
            }

            return result;
        }
    }
}
