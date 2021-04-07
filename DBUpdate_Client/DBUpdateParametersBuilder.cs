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
        public DBUpdateParametersBuilder()
        {
            Reset();
        }

        public DBUpdateParametersBuilder Reset()
        {
            this.isTest = false;
            return this;
        }

        public DBUpdateParametersBuilder SetIsTest(bool value)
        {
            this.isTest = value;
            return this;
        }
     
        public DBUpdateParameters Build()
        {
            return new DBUpdateParameters(isTest);
        }

    }
}
