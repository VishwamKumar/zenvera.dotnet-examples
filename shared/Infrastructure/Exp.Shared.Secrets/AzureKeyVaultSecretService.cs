namespace Exp.Shared.Secrets;

internal sealed class AzureKeyVaultSecretService(
    KeyVaultSettings settings,
    IMemoryCache cache,
    ILogger<AzureKeyVaultSecretService> logger) : ISecretService
{
    public async Task<string?> GetSecretValueAsync(string secretName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secretName);

        if (cache.TryGetValue(secretName, out string? cached) && cached is not null)
        {
            return cached;
        }

        var azure = GetAzureSettings();
        if (string.IsNullOrWhiteSpace(azure.KeyVaultUrl))
        {
            throw new InvalidOperationException(
                "Azure Key Vault is enabled but KeyVaultSettings:Providers:Azure:KeyVaultUrl is empty. Set EXP_AZURE_KEY_VAULT_URL or disable FeatureFlags:UseKeyVault.");
        }

        var client = new SecretClient(new Uri(azure.KeyVaultUrl), CreateCredential(azure));
        logger.LogInformation("Retrieving secret {SecretName} from {VaultUrl}", secretName, azure.KeyVaultUrl);
        var response = await client.GetSecretAsync(secretName, cancellationToken: cancellationToken);
        var value = response.Value.Value;

        if (settings.CacheExpiryMinutes > 0)
        {
            var options = new MemoryCacheEntryOptions();
            var lifetime = TimeSpan.FromMinutes(settings.CacheExpiryMinutes);
            if (settings.UseSlidingExpiration)
            {
                options.SetSlidingExpiration(lifetime);
            }
            else
            {
                options.SetAbsoluteExpiration(lifetime);
            }

            cache.Set(secretName, value, options);
        }

        return value;
    }

    private AzureKeyVaultProviderSettings GetAzureSettings()
        => settings.Providers.TryGetValue("Azure", out var azure)
            ? azure
            : new AzureKeyVaultProviderSettings();

    private static TokenCredential CreateCredential(AzureKeyVaultProviderSettings azure)
    {
        if (!string.IsNullOrWhiteSpace(azure.TenantId)
            && !string.IsNullOrWhiteSpace(azure.ClientId)
            && !string.IsNullOrWhiteSpace(azure.ClientSecret))
        {
            return new ClientSecretCredential(azure.TenantId, azure.ClientId, azure.ClientSecret);
        }

        return new DefaultAzureCredential();
    }
}
