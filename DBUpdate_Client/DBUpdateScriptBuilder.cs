namespace DBUpdate_Client
{
    public class DBUpdateScriptBuilder
    {
        private string name;
        private string path;

        public DBUpdateScriptBuilder()
        {
            Reset();
        }

        public DBUpdateScriptBuilder Reset()
        {
            this.name = null;
            this.path = null;

            return this;
        }

        public DBUpdateScriptBuilder SetName(string value)
        {
            this.name = value;

            return this;
        }
        public DBUpdateScriptBuilder SetPath(string value)
        {
            this.path = value;

            return this;
        }

        public DBUpdateScript Build()
            => new DBUpdateScript(name, path);
    }
}
