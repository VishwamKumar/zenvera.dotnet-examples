namespace Exp.Todo.Application.Features.TodoManager.Command.UpdateTodo;

public class UpdateTodoCommandHandler(
    ITodoReadRepository readRepo,
    ITodoWriteRepository writeRepo   
) : ICommandHandler<UpdateTodoCommand, bool>
{
    public async Task<bool> Handle(UpdateTodoCommand command, CancellationToken cancellationToken = default)
    {
        var dto = command.UpdateDto;
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