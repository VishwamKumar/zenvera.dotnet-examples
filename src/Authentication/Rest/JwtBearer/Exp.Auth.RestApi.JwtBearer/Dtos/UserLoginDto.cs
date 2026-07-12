
namespace Exp.Auth.RestApi.JwtBearer.Dtos;

public class UserLoginDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
