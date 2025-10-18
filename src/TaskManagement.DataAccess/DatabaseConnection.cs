using Microsoft.Data.SqlClient;

namespace TaskManagement.DataAccess;

public class DatabaseConnection : IDatabaseConnection
{
    public Dictionary<string, string> Mapper { get; } = [];
    public string ConnectionString { get; set; } = string.Empty;
    public SqlConnection GetSqlConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}