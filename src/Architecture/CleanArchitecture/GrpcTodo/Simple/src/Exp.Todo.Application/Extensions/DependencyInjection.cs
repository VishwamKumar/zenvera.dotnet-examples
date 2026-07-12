
namespace Exp.Todo.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register all validators from the Application assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register the TodoService
        services.AddScoped<ITodoService, TodoService>();

        services.AddHttpContextAccessor();

        // AutoMapper with current domain assemblies
        services.AddAutoMapper(config =>
        {
            config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
        });

        return services;
    }
}

