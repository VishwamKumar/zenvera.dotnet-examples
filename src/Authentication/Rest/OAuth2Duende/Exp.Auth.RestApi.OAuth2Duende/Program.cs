var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure settings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<IdentityServerSettings>(builder.Configuration.GetSection("IdentityServerSettings"));

// Retrieve and register configuration instances
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var identityServerSettings = builder.Configuration.GetSection("IdentityServerSettings").Get<IdentityServerSettings>();

builder.Services.AddSingleton(jwtSettings!);
builder.Services.AddSingleton(identityServerSettings!);

// Generate Symmetric Key - But it is not needed right now
//var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.SecretKey));

// Generate RSA Key
RSAParameters rsaParameters = RSAKeyProvider.GetRSAParameters();
var rsaSecurityKey = new RsaSecurityKey(rsaParameters)
{
    KeyId = jwtSettings!.SecretKey 
};
var signingCredentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);


// Add IdentityServer with in-memory clients and resources
builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = false;
})
//.AddInMemoryClients(ClientUserProvider.GetClients()) - TBD
.AddInMemoryClients(
[
    new Client
    {
        ClientId = identityServerSettings!.ClientId,
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        ClientSecrets = { new Secret(identityServerSettings.ClientSecret.Sha256()) },
        AllowedScopes = identityServerSettings.Scopes.Select(s => s.Name).ToList(),
        AccessTokenLifetime = jwtSettings.ExpiryInMinutes*60
    }
    //,new Client
    //{
    //    ClientId = "WeatherAppClient2",
    //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
    //    ClientSecrets = { new Secret(identityServerSettings.ClientSecret.Sha256()) },
    //    AllowedScopes = identityServerSettings.Scopes.Select(s => s.Name).ToList(),
    //    AccessTokenLifetime = jwtSettings.ExpiryInMinutes*60
    //}
])
.AddInMemoryApiScopes(identityServerSettings.Scopes.Select(s => new ApiScope
{
    Name = s.Name,
    DisplayName = s.DisplayName
}).ToList())
.AddInMemoryApiResources(
[
    new ApiResource(jwtSettings.Audience)
    {
        Scopes = identityServerSettings.Scopes.Select(s => s.Name).ToList()
    }
])
//.AddDeveloperSigningCredential(); // This is for development purposes only
//.AddTestUsers(ClientUserProvider.GetTestUsers())
.AddSigningCredential(signingCredentials); //Need to ensure both IdentityServer and JWT uses same key


// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.UseSecurityTokenValidators = true;
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Authority = jwtSettings!.Issuer;
    options.Audience = jwtSettings.Audience; // Set your expected audience here                                            
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime =false, // Setting it to false, as it throws an Error, even though value is there. Error: Authentication failed: IDX10225: Lifetime validation failed.The token is missing an Expiration Time.Tokentype: 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken'.
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, // Or TimeSpan.FromMinutes(5)
        IssuerSigningKey = rsaSecurityKey,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience, // Ensure this matches options.Audience           
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("Authentication failed: {error}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated for user {user}", context?.Principal?.Identity?.Name??"NA");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

//Incase Role based requirement is there
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("WeatherReadPolicy", policy =>
//        policy.RequireRole("weather_read"));

//    options.AddPolicy("WeatherWritePolicy", policy =>
//        policy.RequireRole("weather_write"));
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
