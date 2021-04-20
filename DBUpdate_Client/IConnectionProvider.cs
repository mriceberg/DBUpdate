using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public interface IConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
