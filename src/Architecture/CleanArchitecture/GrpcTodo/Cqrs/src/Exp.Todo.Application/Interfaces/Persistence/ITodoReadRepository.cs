
namespace Exp.Todo.Application.Interfaces.Persistence;

public interface ITodoReadRepository
{
    Task<IEnumerable<TodoEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
