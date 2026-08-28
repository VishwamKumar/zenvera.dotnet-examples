namespace Exp.Shared.Secrets;

public sealed class KeyVaultSettings
{
    public string Provider { get; set; } = "Azure";
    public int CacheExpiryMinutes { get; set; } = 30;
    public bool UseSlidingExpiration { get; set; } = true;
    public Dictionary<string, AzureKeyVaultProviderSettings> Providers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class AzureKeyVaultProviderSettings
{
    public string? KeyVaultUrl { get; set; }
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
}
