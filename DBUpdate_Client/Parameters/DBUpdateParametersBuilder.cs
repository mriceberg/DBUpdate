﻿using System;
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
        private string isUpToBlock;
        private string isBlockName;
        private bool isForce;

        public DBUpdateParametersBuilder()
        {
            Reset();
        }
        public DBUpdateParametersBuilder Reset()
        {
            this.isTest = false;
            this.isSilent = false;
            this.isSimulation = false;
            this.isUpToBlock = "";
            this.isBlockName = "";
            this.isForce = false;
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
        public DBUpdateParameters Build()
        {
            return new DBUpdateParameters(isTest, isSilent, isSimulation, isUpToBlock, isBlockName, isForce);
        }

    }
}
