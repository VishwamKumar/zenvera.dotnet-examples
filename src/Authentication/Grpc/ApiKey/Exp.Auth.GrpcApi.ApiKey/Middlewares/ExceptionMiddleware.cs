namespace Exp.Auth.GrpcApi.ApiKey.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (RpcException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            var rpcException = new RpcException(new Status(StatusCode.Unknown, ex.Message));
            await HandleExceptionAsync(context, rpcException);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, RpcException exception)
    {
        context.Response.ContentType = "application/grpc";
        context.Response.StatusCode = StatusCodes.Status200OK; // gRPC status codes are returned via trailers, not HTTP status codes

        var trailers = new Metadata
        {
            { "grpc-status", ((int)exception.StatusCode).ToString() },
            { "grpc-message", exception.Status.Detail }
        };

        foreach (var trailer in trailers)
        {
            context.Response.AppendTrailer(trailer.Key, trailer.Value);
        }

        var errorDetails = new
        {
            grpc_status = exception.StatusCode.ToString(),
            grpc_message = exception.Status.Detail
        };

        var response = System.Text.Json.JsonSerializer.Serialize(errorDetails);
        return context.Response.WriteAsync(response);
    }
}
