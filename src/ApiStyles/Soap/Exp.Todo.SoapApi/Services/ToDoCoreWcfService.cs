namespace Exp.Todo.SoapApi.Services;

public class ToDoCoreWcfService(ITodoService todoService, ILogger<ToDoService> logger, IMapper mapper) : IToDoCoreWcfService
{
   
    public async Task<IEnumerable<ToDoResponse>> GetToDosAsync()
    {
        var recs = await todoService.GetToDosAsync();
        return mapper.Map<List<ToDoResponse>>(recs);
    }

    public async Task<ToDoResponse?> GetToDoByIdAsync(int id)
    {
        var toDo = await todoService.GetToDoByIdAsync(id);
        return toDo == null ? null : mapper.Map<ToDoResponse>(toDo);
    }

    public async Task<ToDoResponse?> AddToDoAsync(ToDoRequest request)
    {
        try
        {
            var toDo = new ToDo { ToDoName = request.ToDoName };
            var addedToDo = await todoService.AddToDoAsync(toDo);
            return mapper.Map<ToDoResponse>(addedToDo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateToDoAsync(int id, ToDoRequest request)
    {
        try
        {
            var toDo = await todoService.GetToDoByIdAsync(id);
            if (toDo == null)
            {
                return false;
            }
            toDo.ToDoName = request.ToDoName;
            await todoService.UpdateToDoAsync(toDo);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteToDoAsync(int id)
    {
        try
        {
            var toDo = await todoService.GetToDoByIdAsync(id);
            if (toDo == null)
            {
                return false;
            }
            await todoService.DeleteToDoByIdAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
