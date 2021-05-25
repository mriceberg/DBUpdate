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
        private bool isSilent;
        private bool isSimulation;

        public DBUpdateParametersBuilder()
        {
            Reset();
        }
        public DBUpdateParametersBuilder Reset()
        {
            this.isTest = false;
            this.isSilent = false;
            this.isSimulation = false;
            return this;
        }
        public DBUpdateParametersBuilder SetIsTest(bool value)
        {
            this.isTest = value;
            return this;
        }
        public DBUpdateParametersBuilder SetIsSilent(bool value)
        {
            this.isSilent = value;
            return this;
        }
        public DBUpdateParametersBuilder SetIsSimulation(bool value)
        {
            this.isSimulation = value;
            return this;
        }
        public DBUpdateParameters Build()
        {
            return new DBUpdateParameters(isTest, isSilent, isSimulation);
        }

    }
}
