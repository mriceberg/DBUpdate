using System.Collections.Generic;
using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public class ScriptGateway
    {
        private readonly ConnectionProvider connectionProvider;

        public ScriptGateway(ConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public IEnumerable<string> GetExecutedScriptNames()
        {
            IList<string> alreadyExecuted = new List<string>();

            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = @"SELECT DISTINCT BlockName FROM dbupdate.Script";

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string blockName = reader.GetString(0);
                            alreadyExecuted.Add(blockName);
                        }
                    }
                }
            }

            return alreadyExecuted;
        }
    }
}
