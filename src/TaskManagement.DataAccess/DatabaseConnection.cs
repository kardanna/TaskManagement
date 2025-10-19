using Microsoft.Data.SqlClient;
using TaskManagement.Domain;

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