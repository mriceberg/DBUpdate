using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public interface ConnectionProvider
    {
        SqlConnection GetConnection();
    }
}
