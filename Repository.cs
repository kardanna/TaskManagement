using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Dapper;

namespace TaskManagement;

class Repository<T>(IDatabaseConnection dbConnection) : IRepository<T> where T : class
{
    private readonly IDatabaseConnection _dbConnection = dbConnection;
    private readonly Lazy<PropertyInfo[]> _entityFieldsExceptPK =
        new(
            typeof(T)
                .GetProperties()
                .Where(p => p.CustomAttributes.All(a => !(a.AttributeType == typeof(KeyAttribute))))
                .ToArray()
        );
    private readonly Lazy<string> _PKFieldName =
        new(
            typeof(T)
                .GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute)))
                .First()
                .Name
        );

    public void Add(T entry)
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }
        
        using var connection = _dbConnection.GetSqlConnection();

        var parameters = new DynamicParameters(entry);

        var sb = new StringBuilder($"INSERT INTO {table} (");
        foreach (var field in _entityFieldsExceptPK.Value)
        {
            sb.Append($"{field.Name}, ");
        }
        sb.Length -= 2;
        sb.Append(") VALUES (");
        foreach (var field in _entityFieldsExceptPK.Value)
        {
            sb.Append($"@{field.Name}, ");
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

        var sql = $"DELETE FROM {table} WHERE {_PKFieldName.Value} = @idInput";
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

        var sql = $"SELECT * FROM {table} WHERE {_PKFieldName.Value} = @idInput";
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

        var parameters = new DynamicParameters(entry);

        var sb = new StringBuilder($"UPDATE {table} SET ");
        foreach (var field in _entityFieldsExceptPK.Value)
        {
            sb.Append($"{field.Name} = @{field.Name}, ");
        }
        sb.Length -= 2;
        sb.Append($" WHERE {_PKFieldName.Value} = @{_PKFieldName.Value};");

        connection.Execute(sb.ToString(), parameters);
    }
}