using System.Collections.Generic;
using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public class ScriptGateway
    {
        private readonly IConnectionProvider connectionProvider;

        public ScriptGateway(IConnectionProvider connectionProvider)
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
                            int myOrdinal = reader.GetOrdinal("BlockName");
                            string blockName = reader.GetString(myOrdinal);
                            alreadyExecuted.Add(blockName);
                        }
                    }
                }
            }

            return alreadyExecuted;
        }
        public void RecordExecution(int runId, string blockName, string scriptPath)
        {
            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = @"INSERT INTO dbupdate.Script (RunId, BlockName, ScriptName) VALUES (@RunId, @BlockName, @ScriptName);";
                    command.Parameters.AddWithValue("@RunId", runId);
                    command.Parameters.AddWithValue("@BlockName", blockName);
                    command.Parameters.AddWithValue("@ScriptName", scriptPath);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
