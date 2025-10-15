namespace TaskManagement;

class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public override string ToString()
    {
        return $"{Id,3} | {Title,-25} | {Description,-50} | {IsCompleted,-6} | {CreatedAt} ";
    }
}