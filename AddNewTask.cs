namespace TaskManagement;

class AddNewTask(IRepository<TaskItem> repository) : IFeature
{
    private readonly IRepository<TaskItem> _repository = repository;

    public string Name { get; init; } = "Add new task";

    public void Execute()
    {
        TaskItem newTask = new()
        {
            Title = UserInput.PromtString("Enter task title: ", "Title cannot be empty. Please, try again: ", nameof(newTask.Title)),
            Description = UserInput.PromtString("Enter task description: ", "Description cannot be empty. Please, try again: ", nameof(newTask.Description)),
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        _repository.Add(newTask);
    }
}