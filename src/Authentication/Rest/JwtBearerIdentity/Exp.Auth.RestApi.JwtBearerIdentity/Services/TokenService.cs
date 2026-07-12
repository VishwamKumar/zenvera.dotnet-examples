namespace Exp.Auth.RestApi.JwtBearerIdentity.Services;

public class TokenService()
{
    public string CreateToken(ApplicationUser user, JwtSettings jwtSettings)
    {
        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //Add Claims here
        var claims = CreateClaims(user);

        var newtoken = new JwtSecurityToken(
          issuer: jwtSettings.Issuer,
          audience: jwtSettings.Audience,
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryInMinutes),
          signingCredentials: credentials);

        return tokenHandler.WriteToken(newtoken);
    }


    private static List<Claim> CreateClaims(ApplicationUser user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, "TokenForTheExp.Auth.RestApi.JwtBearerIdentity", ClaimValueTypes.String),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
                new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user?.UserName??"NA"),
                new (ClaimTypes.Email, user?.Email??"NA"),
                new ("UserId", user!.AppUserId.ToString())

            };
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }  
}