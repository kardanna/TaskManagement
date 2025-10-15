namespace TaskManagement;

class DeleteTask(IRepository<TaskItem> repository) : IFeature
{
    private readonly IRepository<TaskItem> _repository = repository;

    public string Name { get; init; } = "Delete a task";

    public void Execute()
    {
        var task = _repository.Get(
            UserInput.PromtInt("Enter task # to delete: ")
            );

        if (task == null)
        {
            Console.WriteLine("No task found with specified #");
            return;
        }

        _repository.Delete(task.Id);
        Console.WriteLine($"Task #{task.Id} is successfully deleted!");
    }
}