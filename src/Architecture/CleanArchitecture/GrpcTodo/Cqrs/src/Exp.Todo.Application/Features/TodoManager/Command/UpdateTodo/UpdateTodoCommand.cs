namespace Exp.Todo.Application.Features.TodoManager.Command.UpdateTodo;

public record UpdateTodoCommand(UpdateTodoDto UpdateDto) : ICommand<bool>;

