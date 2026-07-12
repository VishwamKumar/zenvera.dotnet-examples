namespace Exp.Todo.Application.Features.TodoManager.Command.CreateTodo;

public class CreateTodoCommandHandler(
    ITodoWriteRepository writeRepo   
) : IRequestHandler<CreateTodoCommand, int>
{
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateDto;
        TodoEntity todo = TodoEntity.Create(dto.TodoName);
        var id = await writeRepo.AddAsync(todo, cancellationToken);
        return id;        
    }
}