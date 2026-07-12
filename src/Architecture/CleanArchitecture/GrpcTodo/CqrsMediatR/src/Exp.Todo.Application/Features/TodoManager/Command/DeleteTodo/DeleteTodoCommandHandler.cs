namespace Exp.Todo.Application.Features.TodoManager.Command.DeleteTodo;

public class DeleteTodoCommandHandler(
    ITodoReadRepository readRepo,
    ITodoWriteRepository writeRepo
) : IRequestHandler<DeleteTodoCommand, bool>
{
    public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await readRepo.GetByIdAsync(request.Id, cancellationToken);
        if (todo == null)
            return false;
        
        return await writeRepo.DeleteAsync(request.Id, cancellationToken);
    }
}
