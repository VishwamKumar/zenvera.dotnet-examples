namespace Exp.Auth.RestApi.OAuth2Duende.Providers;

public static class RSAKeyProvider
{
    private static RSAParameters rsaParameters;
    static RSAKeyProvider()
    {
        // Initialize RSA key pair once during application startup
        using RSA rsa = RSA.Create();
        {
            rsaParameters = rsa.ExportParameters(true); // Export both public and private key parts
        }
    }

    public static RSAParameters GetRSAParameters()
    {
        return rsaParameters;
    }
}

