namespace Exp.Shared.Secrets;

public interface ISecretService
{
    Task<string?> GetSecretValueAsync(string secretName, CancellationToken cancellationToken = default);
}
