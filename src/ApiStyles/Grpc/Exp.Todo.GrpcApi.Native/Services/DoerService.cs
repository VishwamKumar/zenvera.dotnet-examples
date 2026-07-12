
namespace Exp.Todo.GrpcApi.Native.Services;

public class DoerService(ILogger<DoerService> logger, ITodoService todoService, IMapper mapper) : Doer.DoerBase
{

    public override async Task<ToDoReply> GetToDo(ToDoIdRequest request, ServerCallContext context)
    {
        logger.LogInformation("Received request for ToDo item with Id: {Id}", request.Id);
        var todoItem = await todoService.GetToDoByIdAsync(request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"ToDo item with Id {request.Id} not found"));
        var response = mapper.Map<ToDoReply>(todoItem);
        return response;
    }

    public override async Task<ToDosReply> GetToDos(Empty request, ServerCallContext context)
    {
        logger.LogInformation("Received request for all ToDo items.");

        var todoItems = await todoService.GetToDosAsync();
        var response = new ToDosReply();

        if (todoItems != null)
        {
            response.Todos.AddRange(mapper.Map<IEnumerable<ToDoReply>>(todoItems));
        }

        return response;
    }

    public override async Task<ToDoReply> AddToDo(ToDoNameRequest request, ServerCallContext context)
    {
        try
        {
            ToDo toDo = new() { ToDoName = request.Todoname };
            var todoItem = await todoService.AddToDoAsync(toDo) ?? throw new RpcException(new Status(StatusCode.Unknown, $"ToDo item with ToDoName {request.Todoname} could not be added."));
            var response = mapper.Map<ToDoReply>(todoItem);
            return response;
        }
        catch (RpcException)
        {
            throw; // Re-throw RpcException to propagate it as-is
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while adding ToDo item with ToDoName: {request.Todoname}");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred while processing the request."));
        }
    }

    public override async Task<StatusToDoReply> UpdateToDo(ToDoRequest request, ServerCallContext context)
    {
        try
        {
            StatusToDoReply response = new();
            var existingToDo = await todoService.GetToDoByIdAsync(request.Id) ?? throw new RpcException(new Status(StatusCode.NotFound, $"ToDo item with Id {request.Id} not found"));
            existingToDo.ToDoName = request.Todoname;
            var updatedToDo = await todoService.UpdateToDoAsync(existingToDo);

            if (updatedToDo != null)
            {
                response.Success = true;
            }

            return response;
        }

        catch (RpcException)
        {
            throw; // Re-throw RpcException to propagate it as-is
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while updating ToDo item with Id: {request.Id}");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred while processing the request."));
        }
    }

    public override async Task<StatusToDoReply> DeleteToDoById(ToDoIdRequest request, ServerCallContext context)
    {
        try
        {
            var success = await todoService.DeleteToDoByIdAsync(request.Id);
            var response = new StatusToDoReply
            {
                Success = success
            };

            return response;
        }

        catch (RpcException)
        {
            throw; // Re-throw RpcException to propagate it as-is
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error occurred while deleting ToDo item with Id: {request.Id}");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred while processing the request."));
        }
    }
}


