
namespace Exp.Todo.RestApi.FastEndpoints.Endpoints;

public class DeleteToDoEndpoint(ITodoService todoService, ILogger<UpdateToDoEndpoint> logger) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/todos/{id}");
        AllowAnonymous();      
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var id = Route<int>("id");
            ToDo? toDo = await todoService.GetToDoByIdAsync(id);

            if (toDo == null)
            {
                await SendNotFoundAsync(ct);
            }
            _ = await todoService.DeleteToDoByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
        await SendNoContentAsync(ct);        
    }
}

