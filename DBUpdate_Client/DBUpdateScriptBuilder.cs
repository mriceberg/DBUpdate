namespace DBUpdate_Client
{
    public class DBUpdateScriptBuilder
    {
        private string name;

        public DBUpdateScriptBuilder()
        {
            Reset();
        }

        public DBUpdateScriptBuilder Reset()
        {
            this.name = null;

            return this;
        }

        public DBUpdateScriptBuilder SetName(string value)
        {
            this.name = value;

            return this;
        }

        public DBUpdateScript Build()
            => new DBUpdateScript(name);
    }
}
