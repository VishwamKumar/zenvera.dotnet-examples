namespace Exp.Todo.GrpcApi.Extensions;

public static class SwaggerExtension
{
    public static void ConfigureSwaggerServices(this WebApplicationBuilder builder)
    {
        bool useSwagger = builder.Configuration.GetValue("UseSwagger", false);
        if (!useSwagger) return;

        builder.Services.AddGrpcSwagger();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoEntity API", Version = "v1" });
        });
    }

    public static void UseSwaggerMiddleware(this WebApplication app)
    {
        bool useSwagger = app.Configuration.GetValue("UseSwagger", false);
        if (!useSwagger) return;

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoEntity API v1");
        });
    }

    public static void ConfigureSwaggerEndpoints(this WebApplication app)
    {
        bool useSwagger = app.Configuration.GetValue("UseSwagger", false);

        // Root endpoint - redirect to Swagger if enabled, otherwise show message
        app.MapGet("/", context =>
        {
            if (useSwagger)
            {
                context.Response.Redirect("/swagger");
            }
            else
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
            }
            return Task.CompletedTask;
        });
    }
}
