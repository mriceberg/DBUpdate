using System.Data.SqlClient;

namespace DBUpdate_Client
{
    public class DBUpdateStructureValidator
    {
        private readonly ConnectionProvider connectionProvider;

        public const string SCHEMA_NAME = "dbupdate";
        public const string RUN_TABLE_NAME = "Run";
        public const string SCRIPT_TABLE_NAME = "Script";

        public DBUpdateStructureValidator(ConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider; ;
        }

        public void EnsureStructureExists()
        {
            EnsureSchemaExists();
            EnsureRunTableExists();
            EnsureScriptTableExists();
        }
        private void EnsureSchemaExists()
        {
            bool schemaExists = SchemaExists(SCHEMA_NAME);

            if (!schemaExists)
            {
                CreateSchema(SCHEMA_NAME);
            }
        }
        private void EnsureRunTableExists()
        {
            bool runTableExists = TableExists(SCHEMA_NAME, RUN_TABLE_NAME);

            if (!runTableExists)
            {
                CreateRunTable();
            }
        }
        private void EnsureScriptTableExists()
        {
            bool scriptTableExists = TableExists(SCHEMA_NAME, SCRIPT_TABLE_NAME);

            if (!scriptTableExists)
            {
                CreateScriptTable();
            }
        }
        private void CreateRunTable()
            => CreateTable(SCHEMA_NAME, RUN_TABLE_NAME, "Id INT IDENTITY(1, 1) PRIMARY KEY, StartDate DATETIME NOT NULL DEFAULT GETDATE(), EndDate DATETIME NULL");
        private void CreateScriptTable()
            => CreateTable(SCHEMA_NAME, SCRIPT_TABLE_NAME, "CREATE TABLE dbupdate.Script (Id INT IDENTITY(1, 1) PRIMARY KEY, RunId INT NOT NULL FOREIGN KEY REFERENCES dbupdate.Run(Id), BlockName VARCHAR(200) NOT NULL, ScriptName VARCHAR(200) NOT NULL, ExecutionDate DATETIME NOT NULL DEFAULT GETDATE()");
        private bool SchemaExists(string schemaName)
            => Exists(@"SELECT 1 FROM sys.schemas s WHERE s.name = @SchemaName;", new SqlParameter[] { new SqlParameter("@SchemaName", schemaName) });

        private void CreateSchema(string schemaName)
        {
            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $@"CREATE SCHEMA [{schemaName}];";

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
        private bool TableExists(string schemaName, string tableName)
            => Exists(@"SELECT 1 FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id WHERE t.name = @TableName AND s.name = @SchemaName;",
                      new SqlParameter[] { new SqlParameter("@SchemaName", schemaName), new SqlParameter("@TableName", tableName) });
        private void CreateTable(string schemaName, string tableName, string tableContents)
        {
            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $@"CREATE TABLE [{schemaName}].[{tableName}] ({tableContents});";

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        private bool Exists(string queryText, params SqlParameter[] queryParameters)
        {
            using (var connection = connectionProvider.GetConnection())
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = queryText;

                    foreach(var parameter in queryParameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }
    }
}
