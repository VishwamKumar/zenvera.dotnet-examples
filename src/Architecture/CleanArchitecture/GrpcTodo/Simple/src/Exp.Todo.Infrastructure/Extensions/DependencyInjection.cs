namespace Exp.Todo.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {  
        // Register unified repository
        services.AddScoped<ITodoRepository, TodoRepository>();
        
        var connectionString = configuration.GetConnectionString("SqliteConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("❌ Missing 'SqliteConnection' string in configuration.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        
        return services;
    }
}

