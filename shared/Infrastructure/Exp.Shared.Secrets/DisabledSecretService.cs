namespace Exp.Shared.Secrets;

internal sealed class DisabledSecretService : ISecretService
{
    public Task<string?> GetSecretValueAsync(string secretName, CancellationToken cancellationToken = default)
        => Task.FromResult<string?>(
            $"Key Vault is disabled (FeatureFlags:UseKeyVault=false). Secret '{secretName}' was not retrieved.");
}
