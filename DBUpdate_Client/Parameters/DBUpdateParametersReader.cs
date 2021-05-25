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
            .Build();

        protected bool ReadIsSimulation() =>
            this.args.Contains("--simulation") || this.args.Contains("--sim") || this.args.Contains("--SIMULATION") || this.args.Contains("--SIM");
        protected bool ReadIsTest() =>
            this.args.Contains("--test") || this.args.Contains("-t") || this.args.Contains("--TEST") || this.args.Contains("-T");
        protected bool ReadIsSilent() =>
            this.args.Contains("--silent") || this.args.Contains("-s") || this.args.Contains("--SILENT") || this.args.Contains("-S");
    }
}
