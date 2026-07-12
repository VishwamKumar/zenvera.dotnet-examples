namespace Exp.Todo.Infrastructure.Persistence;

public class TodoRepository(AppDbContext dbContext) : ITodoRepository
{
    // Read operations
    public async Task<IEnumerable<TodoEntity>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Todos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<TodoEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    // Write operations
    public async Task<int> AddAsync(TodoEntity todo, CancellationToken cancellationToken = default)
    {
        await dbContext.Todos.AddAsync(todo, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return todo.Id;
    }

    public async Task<bool> UpdateAsync(TodoEntity todo, CancellationToken cancellationToken = default)
    {
        var exists = await dbContext.Todos.AnyAsync(x => x.Id == todo.Id, cancellationToken);
        if (!exists) return false;

        // Check if entity is already tracked
        var trackedEntity = dbContext.Todos.Local.FirstOrDefault(e => e.Id == todo.Id);
        if (trackedEntity != null)
        {
            // Update the tracked entity instead of attaching a new one
            dbContext.Entry(trackedEntity).CurrentValues.SetValues(todo);
        }
        else
        {
            dbContext.Todos.Attach(todo);
            dbContext.Entry(todo).State = EntityState.Modified;
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var todo = await dbContext.Todos.FindAsync([id], cancellationToken);
        if (todo == null)
            return false;

        dbContext.Todos.Remove(todo);
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}

