namespace Exp.Auth.RestApi.JwtBearerIdentity.Services;

public class UsersContext(DbContextOptions<UsersContext> options) : 
                    IdentityUserContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure AppUserId as ignored for inserts
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.AppUserId)
            .ValueGeneratedOnAdd()
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
