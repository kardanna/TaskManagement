namespace TaskManagement;

public interface IFeature
{
    public string Name { get; init; }
    public void Execute();
}