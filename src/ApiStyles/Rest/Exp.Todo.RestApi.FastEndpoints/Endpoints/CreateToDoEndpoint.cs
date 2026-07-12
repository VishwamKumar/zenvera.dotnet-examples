
namespace Exp.Todo.RestApi.FastEndpoints.Endpoints;

public class CreateToDoEndpoint(ITodoService todoService, ILogger<UpdateToDoEndpoint> logger, AutoMapper.IMapper mapper) : Endpoint<ToDoRequest, ToDoResponse>
{
    public override void Configure()
    {
        Post("/api/todos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ToDoRequest req, CancellationToken ct)
    {
        try
        {
            ToDo toDo = new() { ToDoName = req.ToDoName };
            var toDoRec = await todoService.AddToDoAsync(toDo);
            if (toDoRec != null)
            {
                var toDoDto = mapper.Map<ToDoResponse>(toDo); // Map the entity to DTO
                await HttpContext.Response.SendCreatedAtAsync<GetToDoEndpoint>(new { id = toDo.Id }, toDoDto, cancellation: ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex?.Message);
            throw;
        }
    }

}

