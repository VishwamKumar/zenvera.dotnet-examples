namespace Exp.Todo.RestApi.FastEndpoints.Endpoints;

public class GetToDoEndpoint(ITodoService todoService, AutoMapper.IMapper mapper) : EndpointWithoutRequest<ToDoResponse>
{
    public override void Configure()
    {
        Get("/api/todos/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var toDo = await todoService.GetToDoByIdAsync(id);

        if (toDo == null)
        {
            await HttpContext.Response.SendNotFoundAsync(ct);
            return;
        }

        var toDoDto = mapper.Map<ToDoResponse>(toDo);
        await HttpContext.Response.SendOkAsync(toDoDto, cancellation: ct);
    }
}

