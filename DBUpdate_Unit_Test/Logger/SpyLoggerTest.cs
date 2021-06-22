using DBUpdate_Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBUpdate_Unit_Test.Tests
{
    public class SpyLoggerTest : BaseLogger
    {
        public bool Called { get; private set; } 

        public SpyLoggerTest()
        {
            Called = false;
        }

        protected override void DoLogMessage(string message)
        {
            Called = true;
        }
    }
}
