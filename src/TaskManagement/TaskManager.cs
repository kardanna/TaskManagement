using TaskManagement.DataAccess;
using TaskManagement.Domain;

namespace TaskManagement;

class TaskManager
{
    private readonly IRepository<TaskItem> _repository;

    private Dictionary<int, (string FeatureName, Action Feature)> _features = new();
    private void DisplayAllFeatures()
    {
        Console.WriteLine("Available actions:");
        foreach (var feature in _features)
        {
            Console.WriteLine($"{feature.Key,3}. {feature.Value.FeatureName}");
        }
    }
    
    public TaskManager(IRepository<TaskItem> repository)
    {
        _repository = repository;
        _features.Add(1, ("Add new task", AddNewTask));
        _features.Add(2, ("Print all tasks", PrintAllTasks));
        _features.Add(3, ("Mark task as completed", CompleteTask));
        _features.Add(4, ("Delete a task", DeleteTask));
    }

    private Action PromtAction()
    {
        while (true)
        {
            int number = UserInput.PromtInt("Enter action #: ", "Invalid action number. Please, try again: ", nameof(number));
            if (_features.TryGetValue(number, out (string FeatureName, Action Feature) output)) return output.Feature;
            Console.WriteLine($"No action with #{number}");
        }
    }

    public void Start()
    {
        do
        {
            try
            {
                DisplayAllFeatures();
                PromtAction().Invoke();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"The variable to cause this problem is {e.ParamName}");
                Console.WriteLine("Exiting....");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Exiting...");
                return;
            }

        }
        while (UserInput.PromtYesNo("Do you want to exit (y/n): ") == YesNoResponse.No);
    }

    private void AddNewTask()
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

    public void PrintAllTasks()
    {
        Console.WriteLine($"  # | {"Title",-25} | {"Description",-50} | Compl. | CreatedAt");

        foreach (var entry in _repository.GetAll())
        {
            Console.WriteLine(entry);
        }
    }

    public void CompleteTask()
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

    public void DeleteTask()
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