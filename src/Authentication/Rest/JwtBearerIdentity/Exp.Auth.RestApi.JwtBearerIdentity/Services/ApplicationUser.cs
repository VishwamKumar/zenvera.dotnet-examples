namespace Exp.Auth.RestApi.JwtBearerIdentity.Services;

public class ApplicationUser : IdentityUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AppUserId { get; set; }
}
