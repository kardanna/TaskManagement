using Microsoft.Data.SqlClient;

namespace TaskManagement;

interface IDatabaseConnection
{
    public Dictionary<string, string> Mapper { get; }
    public SqlConnection GetSqlConnection();
}