
namespace Exp.Auth.RestApi.OAuth2Duende.Providers;
public static class ClientUserProvider
{
    public static List<TestUser> GetTestUsers()
    {
        return [
            new TestUser
            {
                SubjectId = "3",
                Username = "TestUser3",
                Password = "development-only-password",
                Claims = 
                [
                    new Claim(JwtClaimTypes.Name, "TestUser3"),
                    new Claim(JwtClaimTypes.GivenName, "TestUser3"),
                    new Claim(JwtClaimTypes.FamilyName, "User"),
                    new Claim(JwtClaimTypes.Email, "TestUser3@example.com")
                ]
            },
            new TestUser
            {
                SubjectId = "4",
                Username = "TestUser4",
                Password = "development-only-password",
                Claims = 
                [
                    new Claim(JwtClaimTypes.Name, "TestUser4"),
                    new Claim(JwtClaimTypes.GivenName, "TestUser4"),
                    new Claim(JwtClaimTypes.FamilyName, "User"),
                    new Claim(JwtClaimTypes.Email, "TestUser4@example.com")
                ]
            }
        ];
    }

    public static IEnumerable<Client> GetClients()
    {
        return [    
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("development-only-client-secret".Sha256())
                },
                AllowedScopes = { "weather_read" }
            }
        ];
    }

}