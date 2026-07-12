namespace Exp.Auth.GrpcApi.JwtBearer.Middlewares;

public class JwtAuthMiddleware(RequestDelegate next, JwtService jwtService)
{
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await next(context);
            return;
        }

        var token = (context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last()) ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "Authorization token is missing."));

        try
        {
            var principal = jwtService.ValidateToken(token) ?? throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token."));

            // If token is valid, continue with the request
            await next(context);
        }
        catch (Exception)
        {
            throw;
        }
    }
}




