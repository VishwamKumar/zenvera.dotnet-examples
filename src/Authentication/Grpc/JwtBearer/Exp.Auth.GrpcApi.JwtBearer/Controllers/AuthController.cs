
namespace Exp.Auth.GrpcApi.JwtBearer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IOptions<List<UserCredential>> userCredentials, 
                    JwtSettings jwtSettings) : ControllerBase
{  
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest login)
    {
       var user = userCredentials.Value.FirstOrDefault(u => u.UserName == login.Username && u.Password == login.Password);
                
        if (user!=null)
        {
            var token = GenerateJwtToken(login.Username);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtSettings.ExpiryInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


