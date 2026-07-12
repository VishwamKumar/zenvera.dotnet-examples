
namespace Exp.Todo.Infrastructure.Persistence;

public class TodoWriteRepository(AppDbContext dbContext) : ITodoWriteRepository
{
    public async Task<int> AddAsync(TodoEntity todo, CancellationToken cancellationToken = default)
    {
        await dbContext.Todos.AddAsync(todo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return todo.Id; 
    }

    public async Task<bool> UpdateAsync(TodoEntity todo, CancellationToken cancellationToken = default)
    {
        // Check if entity is already tracked
        var trackedEntity = dbContext.ChangeTracker.Entries<TodoEntity>()
            .FirstOrDefault(e => e.Entity.Id == todo.Id);

        if (trackedEntity != null)
        {
            // Entity is already tracked, EF Core will detect changes automatically
            // No additional action needed
        }
        else
        {
            // Entity is not tracked (likely from read repository with AsNoTracking)
            // Find the existing entity to ensure it exists and get a tracked instance
            var existing = await dbContext.Todos.FindAsync(todo.Id, cancellationToken);
            if (existing == null)
                return false;

            // Update the tracked entity with the modified TodoName from the parameter
            // Since TodoEntity.Update() already validated and set the TodoName, we copy it
            existing.SetTodoName(todo.TodoName);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var todo = await dbContext.Todos.FindAsync(id, cancellationToken);
        if (todo == null)
            return false;

        dbContext.Todos.Remove(todo);
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}

