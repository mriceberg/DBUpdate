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

        public DBUpdateParameters(bool isTest, bool isSilent)
        {
            IsTest = isTest;
            IsSilent = isSilent;
        }
    }
}
