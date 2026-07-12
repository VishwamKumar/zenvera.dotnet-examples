namespace Exp.Todo.Application.Features.TodoManager.Queries.GetById;

public class GetByIdQueryHandler(ITodoReadRepository readRepo, IMapper mapper) : IQueryHandler<GetByIdQuery, TodoDto?>
{
    public async Task<TodoDto?> Handle(GetByIdQuery query, CancellationToken cancellationToken = default)
    {
        var todo = await readRepo.GetByIdAsync(query.Id, cancellationToken);
        return todo is null ? null : mapper.Map<TodoDto>(todo);
    }
}

