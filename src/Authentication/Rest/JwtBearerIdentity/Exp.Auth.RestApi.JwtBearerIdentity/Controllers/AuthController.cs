
namespace Exp.Auth.RestApi.JwtBearerIdentity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserManager<ApplicationUser> userManager, UsersContext context, TokenService tokenService, JwtSettings jwtSettings) : ControllerBase
{   
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);  
        }

        var result = await userManager.CreateAsync(
            new ApplicationUser { UserName = request.Username, Email = request.Email},
            request.Password
        );

        if (result.Succeeded)
        {
            request.Password = "";
            return CreatedAtAction(nameof(Register), new {email = request.Email}, request);
        }
        
        foreach (var error in result.Errors) { 
            ModelState.AddModelError(error.Code, error.Description); 
        }
        return BadRequest(ModelState);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var managedUser = await userManager.FindByEmailAsync(request.Email);
        
        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(managedUser, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }
        
        var userInDb = context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userInDb is null)
            return Unauthorized();
        
        var accessToken = tokenService.CreateToken(userInDb,jwtSettings);
        await context.SaveChangesAsync();
      
        return Ok(new AuthResponse
        {
            Username = userInDb?.UserName??"",
            Email = userInDb?.Email??"",
            Token = accessToken,
        });
    }
}