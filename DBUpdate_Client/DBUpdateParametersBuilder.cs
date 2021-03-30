using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class DBUpdateParametersBuilder
    {
        private bool isTest;
        private bool mustCheck;
        public DBUpdateParametersBuilder()
        {
            Reset();
        }

        public DBUpdateParametersBuilder Reset()
        {
            this.isTest = false;
            this.mustCheck = false;

            return this;
        }

        public DBUpdateParametersBuilder SetIsTest(bool value)
        {
            this.isTest = value;

            return this;
        }

        public DBUpdateParametersBuilder SetMustCheck(bool value)
        {
            this.mustCheck = value;

            return this;
        }

        public DBUpdateParameters Build()
        {
            return new DBUpdateParameters(isTest, mustCheck);
        }

        //protected override bool ReadIsTest() => this.args.Contains("--test") || this.args.Contains("-t") || this.args.Contains("/test");
        //protected override bool ReadMustCheck() => this.args.Contains("--check");

    }
}
