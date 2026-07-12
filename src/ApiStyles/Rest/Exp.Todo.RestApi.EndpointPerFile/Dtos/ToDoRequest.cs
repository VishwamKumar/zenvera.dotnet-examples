namespace Exp.Todo.RestApi.EndpointPerFile.Dtos;

public class ToDoRequest
{
    [Required]
    public string ToDoName { get; set; } = null!;
}
