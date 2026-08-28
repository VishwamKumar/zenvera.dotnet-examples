namespace Exp.Shared.Secrets;

public static class SecretsServiceCollectionExtensions
{
    public static IServiceCollection AddSecretsService(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var flags = configuration.GetFeatureFlags();
        var settings = configuration.BindResolved<KeyVaultSettings>("KeyVaultSettings");
        services.AddSingleton(settings);
        services.AddMemoryCache();

        if (!flags.UseKeyVault)
        {
            services.AddSingleton<ISecretService, DisabledSecretService>();
            return services;
        }

        services.AddSingleton<ISecretService, AzureKeyVaultSecretService>();
        return services;
    }
}
