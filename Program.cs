using dotenv.net;

namespace TaskManagement;

class Program
{
    static void Main(string[] args)
    {
        var manager = new TaskManager();

        DotEnv.Load();
        manager.SetDbConnectionString(Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "");
        manager.MapTaskEnityTo("dbo.Tasks");

        manager.Start();
    }
}