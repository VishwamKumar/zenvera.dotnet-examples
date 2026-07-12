namespace Exp.Todo.GrpcApi.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            // Re-throw RpcException as-is
            throw;
        }
        catch (AppValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error in {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                $"Validation failed: {string.Join(", ", ex.Errors)}"));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error in {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                $"Domain validation failed: {string.Join(", ", ex.Errors)}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in {Method}", context.Method);
            throw new RpcException(new Status(StatusCode.Internal,
                "An unexpected error occurred. Please try again later."));
        }
    }
}

