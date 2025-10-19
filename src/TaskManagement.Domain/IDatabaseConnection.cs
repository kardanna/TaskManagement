using Microsoft.Data.SqlClient;

namespace TaskManagement.Domain;

public interface IDatabaseConnection
{
    public Dictionary<string, string> Mapper { get; }
    public string ConnectionString { get; set; }
    public SqlConnection GetSqlConnection();
}