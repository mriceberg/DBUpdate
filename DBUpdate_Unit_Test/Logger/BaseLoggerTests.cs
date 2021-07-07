using DBUpdate_Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DBUpdate_Unit_Test.Logger
{
    public class BaseLoggerTests : BaseLogger
    {

        protected override void DoLogMessage(string message)
        {
            // Récuper le text writer et savoir si il est null ou pas
        }

    }
}
