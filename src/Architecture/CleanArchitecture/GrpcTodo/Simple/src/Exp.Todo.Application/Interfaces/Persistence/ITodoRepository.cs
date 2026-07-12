namespace Exp.Todo.Application.Interfaces.Persistence;

public interface ITodoRepository
{
    // Read operations
    Task<IEnumerable<TodoEntity>?> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    
    // Write operations
    Task<int> AddAsync(TodoEntity todo, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TodoEntity todo, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

