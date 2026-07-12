namespace Exp.Todo.RestApi.MvcControllers.Dtos;

public class ToDoRequest
{
    [Required]
    public string ToDoName { get; set; } = null!;
}
