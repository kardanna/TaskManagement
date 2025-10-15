using dotenv.net;

namespace TaskManagement;

class Program
{
    static void Main(string[] args)
    {
        var manager = new TaskManager();

        manager.AddFeature(new AddNewTask(manager.GetTaskRepository()));
        manager.AddFeature(new PrintAllTasks(manager.GetTaskRepository()));
        manager.AddFeature(new CompleteTask(manager.GetTaskRepository()));
        manager.AddFeature(new DeleteTask(manager.GetTaskRepository()));

        DotEnv.Load();
        manager.SetDbConnectionString(Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "");
        manager.MapTaskEnityTo("dbo.Tasks");

        manager.Start();
    }
}