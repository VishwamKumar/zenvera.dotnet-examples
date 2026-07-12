namespace Exp.Todo.Application.Services;

public interface ITodoService
{
    Task<List<TodoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(CreateTodoDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(UpdateTodoDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

