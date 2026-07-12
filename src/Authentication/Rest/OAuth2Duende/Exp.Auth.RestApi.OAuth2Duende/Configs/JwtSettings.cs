namespace Exp.Auth.RestApi.OAuth2Duende.Configs;

public class JwtSettings
{
    public string Authority { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiryInMinutes { get; set; }
}
