namespace Exp.Auth.GraphQLApi.JwtBearer.Middlewares;

public class JwtAuthMiddleware(FieldDelegate next)
{

    public async Task InvokeAsync(IMiddlewareContext context)
    {
        // Retrieve user information from the context
        var user = context.ContextData["User"] as ClaimsPrincipal;

        // Check if user is authenticated
        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.ReportError("Unauthorized");
            return;
        }

        // Proceed with the next middleware/resolver
        await next(context);
    }
}



