namespace Exp.Todo.GrpcApi.Middleware;

/// <summary>
/// Global exception handling middleware for gRPC services.
/// Catches all unhandled exceptions and converts them to appropriate gRPC status codes.
/// </summary>
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
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? "Unknown";
            _logger.LogError(ex, 
                "An unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}", 
                correlationId, 
                context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            AppValidationException => StatusCode.InvalidArgument,
            DomainException => StatusCode.InvalidArgument,
            ArgumentNullException => StatusCode.InvalidArgument,
            ArgumentException => StatusCode.InvalidArgument,
            KeyNotFoundException => StatusCode.NotFound,
            UnauthorizedAccessException => StatusCode.PermissionDenied,
            InvalidOperationException => StatusCode.FailedPrecondition,
            NotSupportedException => StatusCode.Unimplemented,
            TimeoutException => StatusCode.DeadlineExceeded,
            _ => StatusCode.Internal
        };

        var message = exception is AppValidationException or DomainException
            ? string.Join("; ", GetErrorMessages(exception))
            : GetSafeErrorMessage(exception, statusCode);

        var status = new Status(statusCode, message);
        var rpcException = new RpcException(status);

        // Re-throw as RpcException so gRPC can handle it properly
        throw rpcException;
    }

    private static IEnumerable<string> GetErrorMessages(Exception exception)
    {
        return exception switch
        {
            AppValidationException appEx => appEx.Errors,
            DomainException domainEx => domainEx.Errors,
            _ => [exception.Message]
        };
    }

    private static string GetSafeErrorMessage(Exception exception, StatusCode statusCode)
    {
        // In production, don't expose internal exception details
        return statusCode == StatusCode.Internal
            ? "An internal server error occurred. Please try again later."
            : exception.Message;
    }
}

