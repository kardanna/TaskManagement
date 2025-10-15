using Microsoft.Data.SqlClient;

namespace TaskManagement;

class DatabaseConnection : IDatabaseConnection
{
    public Dictionary<string, string> Mapper { get; } = [];
    public string ConnectionString = string.Empty;
    public SqlConnection GetSqlConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}