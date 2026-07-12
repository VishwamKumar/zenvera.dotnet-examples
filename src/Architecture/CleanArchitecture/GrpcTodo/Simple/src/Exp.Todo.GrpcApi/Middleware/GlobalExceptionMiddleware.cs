namespace Exp.Todo.GrpcApi.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred. Path: {Path}, Method: {Method}",
                context.Request.Path, context.Request.Method);

            // For gRPC requests, we let the gRPC interceptor handle it
            // For HTTP requests, we can handle here
            if (!context.Request.Path.StartsWithSegments("/grpc"))
            {
                await HandleHttpExceptionAsync(context, ex);
            }
            else
            {
                throw; // Re-throw for gRPC to handle
            }
        }
    }

    private static async Task HandleHttpExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            AppValidationException => 400,
            DomainException => 400,
            _ => 500
        };

        var errors = exception switch
        {
            AppValidationException validationEx => validationEx.Errors,
            DomainException domainEx => domainEx.Errors,
            _ => new List<string> { "An unexpected error occurred." }
        };

        var response = new
        {
            error = new
            {
                message = exception.Message,
                errors = errors
            }
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}

