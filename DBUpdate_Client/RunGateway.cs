using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public class RunGateway
    {
        private readonly ConnectionProvider connectionProvider;

        public RunGateway(ConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public int CreateInstance()
        {
            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"INSERT INTO dbupdate.Run DEFAULT VALUES; SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();

                        int id = reader.GetInt32(0);

                        return id;
                    }
                }
            }
        }
        public void CloseInstance(int runId)
        {
            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = @"UPDATE dbupdate.Run SET EndDate = GETDATE() WHERE Id = @RunId;";
                    command.Parameters.AddWithValue("@RunId", runId);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
