
namespace Exp.Todo.Application.Features.TodoManager.Queries.GetById;

public class GetByIdQueryHandler(ITodoReadRepository readRepo, IMapper mapper) : IRequestHandler<GetByIdQuery, TodoDto?>
{
    public async Task<TodoDto?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await readRepo.GetByIdAsync(request.Id, cancellationToken);
        return todo is null ? null : mapper.Map<TodoDto>(todo);
    }
}

