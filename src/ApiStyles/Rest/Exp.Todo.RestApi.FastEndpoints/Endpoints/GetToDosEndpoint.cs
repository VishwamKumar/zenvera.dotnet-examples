
namespace Exp.Todo.RestApi.FastEndpoints.Endpoints;

public class GetToDosEndpoint(ITodoService todoService, AutoMapper.IMapper mapper) : EndpointWithoutRequest<IEnumerable<ToDoResponse>>
{
      public override void Configure()
    {
        Get("/api/todos");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var recs = await todoService.GetToDosAsync();
        var items = mapper.Map<List<ToDoResponse>>(recs);
        await SendOkAsync(items, ct);
    }
        
}
