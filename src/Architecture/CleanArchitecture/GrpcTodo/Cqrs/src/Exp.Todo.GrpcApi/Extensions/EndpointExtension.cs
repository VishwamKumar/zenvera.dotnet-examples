namespace Exp.Todo.GrpcApi.Extensions;

public static class EndpointExtension
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        // Basic health check - indicates the service is running (all checks)
        app.MapHealthChecks("/health");
        
        // Readiness check - indicates the service is ready to accept traffic (database check)
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });
        
        // Liveness check - indicates the service is alive (no checks, just returns 200)
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false // No checks for liveness, just returns 200 if service is running
        });
        
        // gRPC services
        app.MapGrpcService<TodoGrpcService>();    
        app.MapGrpcReflectionService();
        
        // Root endpoint - must be registered last to avoid conflicts
        app.ConfigureSwaggerEndpoints();
    }
}