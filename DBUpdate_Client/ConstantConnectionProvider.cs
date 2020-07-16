using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public class ConstantConnectionProvider : BaseConnectionProvider
    {
        private readonly string connectionString;

        public ConstantConnectionProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override SqlConnection DoGetConnection() => new SqlConnection(this.connectionString);
    }
}
