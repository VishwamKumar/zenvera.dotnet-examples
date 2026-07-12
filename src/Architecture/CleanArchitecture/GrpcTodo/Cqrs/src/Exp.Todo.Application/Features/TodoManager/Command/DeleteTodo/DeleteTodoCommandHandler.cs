namespace Exp.Todo.Application.Features.TodoManager.Command.DeleteTodo;

public class DeleteTodoCommandHandler(
    ITodoReadRepository readRepo,
    ITodoWriteRepository writeRepo
) : ICommandHandler<DeleteTodoCommand, bool>
{
    public async Task<bool> Handle(DeleteTodoCommand command, CancellationToken cancellationToken = default)
    {
        var todo = await readRepo.GetByIdAsync(command.Id, cancellationToken);
        if (todo == null)
            return false;
        
        return await writeRepo.DeleteAsync(command.Id, cancellationToken);
    }
}
