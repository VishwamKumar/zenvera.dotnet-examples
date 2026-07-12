
namespace Exp.Todo.Application.Interfaces.Persistence;

public interface ITodoWriteRepository
{
    Task<int> AddAsync(TodoEntity todo, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TodoEntity todo, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
