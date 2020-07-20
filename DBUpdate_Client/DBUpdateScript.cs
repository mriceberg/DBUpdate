namespace DBUpdate_Client
{
    public class DBUpdateScript
    {
        public string Name { get; private set; }

        public DBUpdateScript(string name)
        {
            this.Name = name;
        }
    }
}
