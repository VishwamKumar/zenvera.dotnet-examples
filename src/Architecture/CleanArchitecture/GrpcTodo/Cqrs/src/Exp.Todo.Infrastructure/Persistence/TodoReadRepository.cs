
namespace Exp.Todo.Infrastructure.Persistence;

public class TodoReadRepository(AppDbContext dbContext) : ITodoReadRepository
{

    public async Task<IEnumerable<TodoEntity>> GetAllAsync(CancellationToken cancellationToken = default)
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

}
