using dotenv.net;
using TaskManagement.DataAccess;
using TaskManagement.Domain;

namespace TaskManagement;

class Program
{
    static void Main(string[] args)
    {

        IDatabaseConnection connection = new DatabaseConnection();
        
        DotEnv.Load();
        connection.ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "";
        connection.Mapper[typeof(TaskItem).ToString()] = "dbo.Tasks";

        IRepository<TaskItem> repository = new Repository<TaskItem>(connection); 
        
        var manager = new TaskManager(repository);

        manager.Start();
    }
}