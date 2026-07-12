namespace Exp.Todo.Application.Features.TodoManager.Command.UpdateTodo;

public class UpdateTodoCommandHandler(
    ITodoReadRepository readRepo,
    ITodoWriteRepository writeRepo   
) : IRequestHandler<UpdateTodoCommand, bool>
{
    public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.UpdateDto;
        var existingRec = await readRepo.GetByIdAsync(dto.Id, cancellationToken);

        if (existingRec != null)
        {
            TodoEntity.Update(existingRec, dto.TodoName);
            await writeRepo.UpdateAsync(existingRec, cancellationToken);
            return true;
        }
        return false;
    }
}