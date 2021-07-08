using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdate_Client
{
    public class DBUpdateParameters
    {
        public bool IsTest { get; private set; }
        public bool IsSilent { get; private set; }
        public bool IsSimulation { get; private set; }
        public string IsUpToBlock { get; private set; }
        public string IsBlockName { get; private set; }
        public bool IsForce { get; private set; }

        public DBUpdateParameters(bool isTest, bool isSilent, bool isSimulation, string isUpToBlock, string isBlockName, bool isForce)
        {
            IsTest = isTest;
            IsSilent = isSilent;
            IsSimulation = isSimulation;
            IsUpToBlock = isUpToBlock;
            IsBlockName = isBlockName;
            IsForce = isForce;
        }
    }
}
