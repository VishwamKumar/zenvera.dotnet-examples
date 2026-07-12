namespace Exp.Todo.GrpcApi.Extensions;

public static class ServiceExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Configure Serilog
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<Interceptors.ExceptionInterceptor>();
        }).AddJsonTranscoding();

        builder.Services.AddGrpcReflection();
        builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();

        builder.Services.AddAutoMapper(config =>
        {
            config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
        });

        builder.Services.AddApplicationServices()
                        .AddInfrastructureServices(builder.Configuration);

        builder.Services.AddGrpc();
    }
}

