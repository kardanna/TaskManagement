namespace TaskManagement;

class CompleteTask(IRepository<TaskItem> repository) : IFeature
{
    private readonly IRepository<TaskItem> _repository = repository;

    public string Name { get; init; } = "Mark task as completed";

    public void Execute()
    {
        var task = _repository.Get(
            UserInput.PromtInt("Enter task # to set as completed: ")
            );

        if (task == null)
        {
            Console.WriteLine("No task found with specified #");
            return;
        }
        
        task.IsCompleted = true;
        _repository.Update(task);
        Console.WriteLine($"Task #{task.Id} is successfully completed!");
    }
}