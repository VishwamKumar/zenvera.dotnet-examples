
namespace Exp.Todo.GraphQLApi.Schemas.Queries;

public class Query(ITodoService todoService, IMapper mapper)
{
   
    public async Task<IEnumerable<ToDoResponse>> GetToDos()
    {
        var recs = await todoService.GetToDosAsync();
        return mapper.Map<List<ToDoResponse>>(recs);
    }

    public async Task<ToDoResponse?> GetToDoById(int id)
    {
        var toDo = await todoService.GetToDoByIdAsync(id);
        return toDo == null ? null : mapper.Map<ToDoResponse>(toDo);
    }
}



