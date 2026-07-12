
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddLogging();
builder.Services.Configure<List<ApiKeyCredential>>(builder.Configuration.GetSection("ApiKeys"));

builder.Services.AddScoped<WeatherForecastQuery>();
//builder.Services.AddScoped<Mutation>();


builder.Services
    .AddGraphQLServer()
    .AddAuthorization() // Enable authorization
    .AddQueryType<WeatherForecastQuery>();    

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());
  
var app = builder.Build();
app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.Run();
