
namespace Exp.Todo.RestApi.FastEndpoints.Endpoints;

public class UpdateToDoEndpoint(ITodoService todoService, ILogger<UpdateToDoEndpoint> logger) : Endpoint<ToDoRequest>
{

    public override void Configure()
    {
        Put("/api/todos/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ToDoRequest req, CancellationToken ct)
    {
        try
        {
            var id = Route<int>("id");
            ToDo? toDo = await todoService.GetToDoByIdAsync(id);

            if (toDo == null)
            {
                await HttpContext.Response.SendNotFoundAsync(ct);
                return;
            }

            toDo!.ToDoName = req.ToDoName;
            var rec = await todoService.UpdateToDoAsync(toDo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }

        await HttpContext.Response.SendNoContentAsync(ct);
    }
}

