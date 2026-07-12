namespace Exp.Auth.RestApi.JwtBearer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IOptions<List<UserCredential>> userCredentials, JwtSettings jwtSettings) : ControllerBase
{

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginDto request)
    {
        var user = userCredentials.Value.FirstOrDefault(u => u.UserName == request.Username && u.Password == request.Password);

        if (user == null)
            return Unauthorized();

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //Add Claims here
        var claims = new[]
        {
                new Claim(ClaimTypes.Name, request.Username)
        };

        var newtoken = new JwtSecurityToken(
          issuer: jwtSettings.Issuer,
          audience: jwtSettings.Audience,
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryInMinutes),
          signingCredentials: credentials);

        var tokenString = tokenHandler.WriteToken(newtoken);

        return Ok(new { Token = tokenString });
    }


}

