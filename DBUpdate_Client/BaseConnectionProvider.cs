using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public abstract class BaseConnectionProvider : ConnectionProvider
    {
        public SqlConnection GetConnection() => DoGetConnection();

        protected abstract SqlConnection DoGetConnection();
    }
}
