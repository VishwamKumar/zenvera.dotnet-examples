namespace Exp.Todo.Application.Dtos;

public class TodoDto
{
    public int Id { get; set; }
    public string TodoName { get; private set; } = default!;   
}

