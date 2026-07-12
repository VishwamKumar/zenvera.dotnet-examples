namespace Exp.Todo.Application.Features.TodoManager.Queries.GetAll;

public class GetAllQueryHandler(ITodoReadRepository readRepository,
    IMapper mapper) : IQueryHandler<GetAllQuery, List<TodoDto>>
{
    public async Task<List<TodoDto>> Handle(GetAllQuery query,
        CancellationToken cancellationToken = default)
    {
        var todos = await readRepository.GetAllAsync(cancellationToken);
        return mapper.Map<List<TodoDto>>(todos);
    }
}
