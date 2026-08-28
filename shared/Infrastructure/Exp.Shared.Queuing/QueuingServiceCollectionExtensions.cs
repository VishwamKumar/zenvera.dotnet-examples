namespace Exp.Shared.Queuing;

public static class QueuingServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqService(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var settings = configuration.BindResolved<RabbitMqSettings>("RabbitMqSettings");
        services.AddSingleton(settings);
        services.AddSingleton<RabbitMqService>();
        services.AddSingleton<IRabbitMqService>(sp => sp.GetRequiredService<RabbitMqService>());
        return services;
    }
}
