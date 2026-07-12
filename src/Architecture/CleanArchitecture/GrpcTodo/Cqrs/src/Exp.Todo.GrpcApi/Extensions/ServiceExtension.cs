namespace Exp.Todo.GrpcApi.Extensions;

public static class ServiceExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Validate configuration on startup
        var databaseOptions = builder.Configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>();
        if (databaseOptions == null || string.IsNullOrWhiteSpace(databaseOptions.SqliteConnection))
        {
            throw new InvalidOperationException(
                $"❌ Configuration Error: '{DatabaseOptions.SectionName}:SqliteConnection' is required but not found or empty. " +
                $"Please provide a valid connection string in appsettings.json or environment variables.");
        }

        // Register and validate options
        builder.Services.AddOptions<DatabaseOptions>()
            .Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddGrpc().AddJsonTranscoding();
        builder.Services.AddGrpcReflection();

        // Add health checks with database check
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>(
                name: "database",
                tags: new[] { "ready", "db" },
                failureStatus: HealthStatus.Unhealthy);

        builder.Services.AddMemoryCache();

        builder.Services.AddApplicationServices()
                        .AddInfrastructureServices(builder.Configuration);
    }
}
