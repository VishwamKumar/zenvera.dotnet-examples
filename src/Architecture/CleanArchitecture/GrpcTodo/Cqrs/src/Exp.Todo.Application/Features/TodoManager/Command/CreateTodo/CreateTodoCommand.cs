namespace Exp.Todo.Application.Features.TodoManager.Command.CreateTodo;

public record CreateTodoCommand(CreateTodoDto CreateDto) : ICommand<int>;

