namespace Exp.Auth.RestApi.OAuth2Duende.Configs;

public class IdentityServerSettings
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public IEnumerable<IdentityScope> Scopes { get; set; } = null!;
}

public class IdentityScope
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
}
