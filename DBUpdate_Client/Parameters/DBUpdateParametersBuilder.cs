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
        private bool isAppend;
		private string isUpToBlock;
        private string isBlockName;
        private bool isForce;
        private string isScan;

        public DBUpdateParametersBuilder()
        {
            Reset();
        }
        public DBUpdateParametersBuilder Reset()
        {
            this.isTest = false;
            this.isSilent = false;
            this.isSimulation = false;
            this.isAppend = false;
            this.isUpToBlock = "";
            this.isBlockName = "";
            this.isForce = false;
            this.isScan = "";

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
        public DBUpdateParametersBuilder SetIsAppend(bool value)
        {
            this.isAppend = value;
            return this;
        }
        public DBUpdateParametersBuilder SetIsUpToBlock(string value)
        {
            this.isUpToBlock = value;
            return this;
        }
        public DBUpdateParametersBuilder SetIsBlockName(string value)
        {
            this.isBlockName = value;
            return this;
        }
        public DBUpdateParametersBuilder SetIsForce(bool value)
        {
            this.isForce = value;
            return this;
        }
        public DBUpdateParametersBuilder SetIsScan(string value)
        {
            this.isScan = value;
            return this;
        }
        public DBUpdateParameters Build()
        {
            return new DBUpdateParameters(isTest, isSilent, isSimulation, isAppend, isUpToBlock, isBlockName, isForce, isScan);
        }

    }
}
