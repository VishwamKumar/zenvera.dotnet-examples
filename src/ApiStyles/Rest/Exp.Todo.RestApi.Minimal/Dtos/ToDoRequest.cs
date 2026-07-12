namespace Exp.Todo.RestApi.Minimal.Dtos;

public class ToDoRequest
{
    [Required]
    public string ToDoName { get; set; } = null!;
}
