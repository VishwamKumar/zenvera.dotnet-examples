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

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });
    }
}

