
namespace Exp.Todo.GrpcApi.Extensions;

public static class EndpointExtension
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapGrpcService<TodoGrpcService>();    
        app.MapGrpcReflectionService();
    }
}