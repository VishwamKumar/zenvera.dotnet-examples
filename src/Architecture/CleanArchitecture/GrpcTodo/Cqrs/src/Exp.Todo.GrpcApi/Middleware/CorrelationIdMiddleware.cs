namespace Exp.Todo.GrpcApi.Middleware;

/// <summary>
/// Middleware to extract or generate correlation IDs for request tracking.
/// Correlation IDs help trace requests across services and logs.
/// </summary>
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeader = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract correlation ID from request header, or generate a new one
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        // Store in context for use throughout the request pipeline
        context.Items["CorrelationId"] = correlationId;

        // Add to response headers so clients can track their requests
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        await _next(context);
    }
}

