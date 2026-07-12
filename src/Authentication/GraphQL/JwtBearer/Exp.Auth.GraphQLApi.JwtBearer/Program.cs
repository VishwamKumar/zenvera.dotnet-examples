
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddScoped<WeatherForecastQuery>();
//builder.Services.AddScoped<Mutation>();

builder.Services.Configure<List<UserCredential>>(builder.Configuration.GetSection("UserCredentials"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings!);


builder.Services
    .AddGraphQLServer()
    .AddAuthorization() // Enable authorization
    .AddQueryType<WeatherForecastQuery>();    

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? "No_Issuer_Signing_Key")),
        ValidIssuer = jwtSettings?.Issuer ?? "No_Valid_Issuer",
        ValidAudience = jwtSettings?.Audience ?? "No_Valid_Audience"
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());
  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL();
app.Run();
