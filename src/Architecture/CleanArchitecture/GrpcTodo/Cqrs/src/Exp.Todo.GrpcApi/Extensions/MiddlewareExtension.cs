namespace Exp.Todo.GrpcApi.Extensions;

public static class MiddlewareExtension
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        // Correlation ID must be first to track all requests
        app.UseMiddleware<CorrelationIdMiddleware>();
        
        // Global exception handling
        app.UseMiddleware<GlobalExceptionMiddleware>();
        
        // Swagger middleware
        app.UseSwaggerMiddleware();
    }
}
