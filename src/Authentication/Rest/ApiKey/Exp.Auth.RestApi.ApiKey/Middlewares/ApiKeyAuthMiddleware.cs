namespace Exp.Auth.RestApi.ApiKey.Middlewares;

public class ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
{

    private readonly List<ApiKeyCredential> apiKeys = configuration.GetSection("ApiKeys").Get<List<ApiKeyCredential>>() ?? []; //This can be changed to database and APIKey will be mapped to a client

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var apiKey = apiKeys.FirstOrDefault(k => k.Key == extractedApiKey);

        if (apiKey == null)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await next(context);
    }

}

