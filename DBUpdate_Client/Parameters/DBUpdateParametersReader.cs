using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class DBUpdateParametersReader
    {
        private readonly string[] args;

        public DBUpdateParametersReader(string[] args)
        {
            this.args = args;
        }

        public DBUpdateParameters Read => new DBUpdateParametersBuilder()
            .SetIsTest(ReadIsTest())
            .SetIsSilent(ReadIsSilent())
            .SetIsSimulation(ReadIsSimulation())
            .SetIsUpToBlock(ReadIsUpToBlock())
            .SetIsBlockName(ReadIsBlockName())
            .SetIsForce(ReadIsForce())
            .Build();

        protected bool ReadIsSimulation() =>
            this.args.Contains("--simulation") || this.args.Contains("--sim") || this.args.Contains("--SIMULATION") || this.args.Contains("--SIM");
        protected bool ReadIsTest() =>
            this.args.Contains("--test") || this.args.Contains("--t") || this.args.Contains("--TEST") || this.args.Contains("--T");
        protected bool ReadIsSilent() =>
            this.args.Contains("--silent") || this.args.Contains("--s") || this.args.Contains("--SILENT") || this.args.Contains("--S");
        protected bool ReadIsForce() =>
            this.args.Contains("--force") || this.args.Contains("--f") || this.args.Contains("--FORCE") || this.args.Contains("--F");

        protected string ReadIsUpToBlock()
        {
            int i = Array.FindIndex(this.args, s => s.ToLower() == "--maxblockname");
            if (i>=0 && this.args.Length > i+1)
            {
                return this.args[i + 1];   
            }
            else
            {
                return "";
            }
        }
        protected string ReadIsBlockName()
        {
            int i = Array.FindIndex(this.args, s => s.ToLower() == "--blockname");
            if (i >= 0 && this.args.Length > i + 1)
            {
                return this.args[i + 1];
            }
            else
            {
                return "";
            }
        }

    }
}
