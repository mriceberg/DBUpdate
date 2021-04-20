using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public abstract class BaseConnectionProvider : IConnectionProvider
    {
        public SqlConnection GetConnection() => DoGetConnection();

        protected abstract SqlConnection DoGetConnection();
    }
}
