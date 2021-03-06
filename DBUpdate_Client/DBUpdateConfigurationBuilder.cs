﻿namespace DBUpdate_Client
{
    public class DBUpdateConfigurationBuilder
    {
        private string workingDirectory;

        public DBUpdateConfigurationBuilder()
        {
            Reset();
        }

        public DBUpdateConfigurationBuilder Reset()
        {
            this.workingDirectory = null;

            return this;
        }

        public DBUpdateConfigurationBuilder SetWorkingDirectory(string value)
        {
            this.workingDirectory = value;

            return this;
        }

        public DBUpdateConfiguration Build() => new DBUpdateConfiguration(workingDirectory);
    }
}
