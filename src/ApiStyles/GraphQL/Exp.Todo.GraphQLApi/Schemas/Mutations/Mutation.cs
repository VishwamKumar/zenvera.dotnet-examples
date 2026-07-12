

namespace Exp.Todo.GraphQLApi.Schemas.Mutations;

public class Mutation(ITodoService todoService, IMapper mapper, ILogger<Mutation> logger)
{
    public async Task<ToDoResponse> AddToDoAsync(ToDoRequest req)
    {
        try
        {
            ToDo toDo = new() { ToDoName = req.ToDoName };
            var toDoRec = await todoService.AddToDoAsync(toDo);
            if (toDoRec == null)
            {
                throw new Exception("The ToDo item could not be created. Please check the input and try again.");
            }
            return mapper.Map<ToDoResponse>(toDoRec);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<bool> UpdateToDoAsync(int id, ToDoRequest req)
    {
        try
        {
            var toDo = await todoService.GetToDoByIdAsync(id);
            if (toDo == null)
            {
                throw new Exception("ToDo item not found.");
            }
            toDo.ToDoName = req.ToDoName;
            var updatedToDo = await todoService.UpdateToDoAsync(toDo);
            return updatedToDo != null;
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
                throw new Exception("ToDo item not found.");
            }
            return await todoService.DeleteToDoByIdAsync(id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
