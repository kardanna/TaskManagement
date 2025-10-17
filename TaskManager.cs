namespace TaskManagement;

class TaskManager
{
    private DatabaseConnection _dbConnection = new();
    public void SetDbConnectionString(string connectionString) => _dbConnection.ConnectionString = connectionString;
    public void MapTaskEnityTo(string tableName) => _dbConnection.Mapper[typeof(TaskItem).ToString()] = tableName;
    public Repository<TaskItem> GetTaskRepository() => new Repository<TaskItem>(_dbConnection);
    
    private Dictionary<int, IFeature> _features { get; } = new();
    public void AddFeature(IFeature feature) => _features.Add(_features.Count + 1, feature);
    private void DisplayAllFeatures()
    {
        Console.WriteLine("Available actions:");
        foreach (var feature in _features)
        {
            Console.WriteLine($"{feature.Key,3}. {feature.Value.Name}");
        }
    }

    private IFeature PromtAction()
    {
        while (true)
        {
            int number = UserInput.PromtInt("Enter action #: ", "Invalid action number. Please, try again: ", nameof(number));
            if (_features.TryGetValue(number, out IFeature? output)) return output;
            Console.WriteLine($"No action with #{number}");
        }
    }

    public void Start()
    {
        if (!_dbConnection.Mapper.TryGetValue(typeof(TaskItem).ToString(), out _) || _dbConnection.ConnectionString == string.Empty)
        {
            Console.WriteLine("Database connection is not configured! Exiting...");
            return;
        }

        do
        {
            try
            {
                DisplayAllFeatures();
                PromtAction().Execute();
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
}