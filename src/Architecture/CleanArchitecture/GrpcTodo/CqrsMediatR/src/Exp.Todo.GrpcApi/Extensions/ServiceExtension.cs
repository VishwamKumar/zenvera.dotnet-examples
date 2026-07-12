namespace Exp.Todo.GrpcApi.Extensions;

public static class ServiceExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc().AddJsonTranscoding();
        builder.Services.AddGrpcReflection();
        builder.Services.AddHealthChecks();
        builder.Services.AddMemoryCache();

        builder.Services.AddApplicationServices()
                        .AddInfrastructureServices(builder.Configuration);
    }
}
