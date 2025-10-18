using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Dapper;

namespace TaskManagement.DataAccess;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IDatabaseConnection _dbConnection;

    private readonly string _tableName;

    private readonly string _selectAllStatement;
    private readonly string _selectByIdStatement;
    private readonly string _deleteStatement;

    private readonly Lazy<string> _insertStatement;
    private readonly Lazy<string> _updateStatement;

    private readonly string _PKFieldName =
        typeof(T)
            .GetProperties()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute)))
            .First()
            .Name;

    private readonly Lazy<PropertyInfo[]> _entityFieldsExceptPK =
        new(
            typeof(T)
                .GetProperties()
                .Where(p => p.CustomAttributes.All(a => !(a.AttributeType == typeof(KeyAttribute))))
                .ToArray()
        );
    
    public Repository(IDatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;

        if (!_dbConnection.Mapper.TryGetValue(typeof(T).ToString(), out string? table))
        {
            throw new ApplicationException($"No table registered for class '{typeof(T)}' in database mapper");
        }

        _tableName = table;

        _deleteStatement = $"DELETE FROM {_tableName} WHERE {_PKFieldName} = @id";
        _selectAllStatement = $"SELECT * FROM {_tableName}";
        _selectByIdStatement = $"SELECT * FROM {_tableName} WHERE {_PKFieldName} = @id";

        _insertStatement = new(() =>
        {
            var sb = new StringBuilder($"INSERT INTO {_tableName} (");
            sb.AppendJoin(", ", _entityFieldsExceptPK.Value.Select(f => f.Name));
            sb.Append(") VALUES (");
            sb.AppendJoin(", ", _entityFieldsExceptPK.Value.Select(f => $"@{f.Name}"));
            sb.Append(')');
            return sb.ToString();
        });

        _updateStatement = new(() =>
        {
            var sb = new StringBuilder($"UPDATE {table} SET ");
            sb.AppendJoin(", ", _entityFieldsExceptPK.Value.Select(f => $"{f.Name} = @{f.Name}"));
            sb.Append($" WHERE {_PKFieldName} = @{_PKFieldName}");
            return sb.ToString();
        });
    }    

    public void Add(T entry)
    {        
        using var connection = _dbConnection.GetSqlConnection();
        var parameters = new DynamicParameters(entry);
        connection.Execute(_insertStatement.Value, parameters);
    }

    public void Delete(int id)
    {
        using var connection = _dbConnection.GetSqlConnection();
        connection.Execute(_deleteStatement, new { id });
    }

    public IEnumerable<T> GetAll()
    {     
        using var connection = _dbConnection.GetSqlConnection();        
        return connection.Query<T>(_selectAllStatement).ToList();
    }

    public T Get(int id)
    {
        using var connection = _dbConnection.GetSqlConnection();
        return connection.QuerySingleOrDefault<T>(_selectByIdStatement, new { id })!;
    }

    public void Update(T entry)
    {
        using var connection = _dbConnection.GetSqlConnection();
        var parameters = new DynamicParameters(entry);
        connection.Execute(_updateStatement.Value, parameters);
    }
}