namespace DBUpdate_Client
{
    public class DBUpdateRun
    {
        private readonly RunGateway gateway;

        public const int NOT_INITIALIZED = -1;

        public int Id { get; private set; }

        public DBUpdateRun(RunGateway gateway)
        {
            this.gateway = gateway;
            this.Id = NOT_INITIALIZED;
        }

        public void Create() => Id = gateway.CreateInstance();
    }
}
