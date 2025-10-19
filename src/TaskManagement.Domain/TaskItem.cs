using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain;

public class TaskItem
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public override string ToString()
    {
        return $"{Id,3} | {Title,-25} | {Description,-50} | {IsCompleted,-6} | {CreatedAt} ";
    }
}