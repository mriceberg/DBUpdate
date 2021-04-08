﻿using System;
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
            .Build();

        protected bool ReadIsTest() =>
            this.args.Contains("--test") || this.args.Contains("-t") || this.args.Contains("--TEST") || this.args.Contains("-T");

    }
}
