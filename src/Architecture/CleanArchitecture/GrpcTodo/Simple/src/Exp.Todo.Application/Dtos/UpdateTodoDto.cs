namespace Exp.Todo.Application.Dtos;

public class UpdateTodoDto
{
    public int Id { get; set; }
    public string TodoName { get; set; } = default!;   
}

