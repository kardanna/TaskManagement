namespace TaskManagement;

class PrintAllTasks(IRepository<TaskItem> repository) : IFeature
{
    private readonly IRepository<TaskItem> _repository = repository;

    public string Name { get; init; } = "Print all tasks";

    public void Execute()
    {
        Console.WriteLine($"  # | {"Title",-25} | {"Description",-50} | Compl. | CreatedAt");
        
        foreach (var entry in _repository.GetAll())
        {
            Console.WriteLine(entry);
        }
    }
}