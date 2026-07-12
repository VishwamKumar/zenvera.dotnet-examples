
namespace Exp.Auth.RestApi.BasicAuthentication.Middlewares;

public class BasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{

    private readonly List<UserCredential> _userCredentials = configuration.GetSection("UserCredentials").Get<List<UserCredential>>() ?? []; //This can be changed to database call or something else for credential validation


    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization header missing");
            return;
        }

        var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader!);

        if (authHeaderValue.Scheme != "Basic")
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Authorization header is not Basic");
            return;
        }

        var credentials = Encoding.UTF8
            .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
            .Split(':', 2);

        if (credentials.Length != 2 || !IsAuthorized(credentials[0], credentials[1]))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid credentials");
            return;
        }

        await next(context);
    }

    private bool IsAuthorized(string username, string password)
    {
        return _userCredentials.Any(u => u.UserName == username && u.Password == password);
    }
}

