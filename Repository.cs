using System.Reflection;
using System.Text;
using Dapper;

namespace TaskManagement;

class Repository<T>(IDatabaseConnection dbConnection) : IRepository<T> where T : class
{
    private readonly IDatabaseConnection _dbConnection = dbConnection;

    public void Add(T entry)
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }
        
        using var connection = _dbConnection.GetSqlConnection();

        PropertyInfo[] properties = typeof(T)
            .GetProperties()
            .Where((f) => !f.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
            .ToArray();

        var parameters = new DynamicParameters(entry);

        var sb = new StringBuilder($"INSERT INTO {table} (");
        foreach (var property in properties)
        {
            sb.Append($"{property.Name}, ");
        }
        sb.Length -= 2;
        sb.Append(") VALUES (");
        foreach (var property in properties)
        {
            sb.Append($"@{property.Name}, ");
        }
        sb.Length -= 2;
        sb.Append(')');

        connection.Execute(sb.ToString(), parameters);
    }

    public void Delete(int id)
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }

        using var connection = _dbConnection.GetSqlConnection();

        string idFieldName = typeof(T)
            .GetProperties()
            .Where((f) => f.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
            .First()
            .Name;

        var sql = $"DELETE FROM {table} WHERE {idFieldName} = @idInput";
        connection.Execute(sql, new { idInput = id });
    }

    public IEnumerable<T> GetAll()
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }
        
        using var connection = _dbConnection.GetSqlConnection();
        
        var sql = $"SELECT * FROM {table}";
        var tasks = connection.Query<T>(sql).ToList();
        return tasks;
    }

    public T Get(int id)
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }

        using var connection = _dbConnection.GetSqlConnection();

        string idFieldName = typeof(T)
            .GetProperties()
            .Where((f) => f.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
            .First()
            .Name;

        var sql = $"SELECT * FROM {table} WHERE {idFieldName} = @idInput";
        var task = connection.QuerySingleOrDefault<T>(sql, new { idInput = id });
        return task!;
    }

    public void Update(T entry)
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }

        using var connection = _dbConnection.GetSqlConnection();

        string idFieldName = typeof(T)
            .GetProperties()
            .Where((f) => f.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
            .First().Name;

        PropertyInfo[] properties = typeof(T)
            .GetProperties()
            .Where((f) => !f.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
            .ToArray();

        var parameters = new DynamicParameters(entry);

        var sb = new StringBuilder($"UPDATE {table} SET ");
        foreach (var property in properties)
        {
            sb.Append($"{property.Name} = @{property.Name}, ");
        }
        sb.Length -= 2;
        sb.Append($" WHERE {idFieldName} = @{idFieldName};");

        connection.Execute(sb.ToString(), parameters);
    }
}