namespace TaskManagement.DataAccess;

public interface IRepository<T> where T : class
{
    public IEnumerable<T> GetAll();
    public T Get(int id);
    public void Add(T entry);
    public void Delete(int id);
    public void Update(T entry);
}