
namespace Exp.Todo.Infrastructure.Sqlite.Services;

public interface ITodoService
{
    Task<IEnumerable<ToDo>?> GetToDosAsync();
    Task<ToDo?> GetToDoByIdAsync(int id);
    Task<ToDo?> AddToDoAsync(ToDo todo);
    Task<ToDo?> UpdateToDoAsync(ToDo todo);
    Task<bool> DeleteToDoByIdAsync(int id);
}
