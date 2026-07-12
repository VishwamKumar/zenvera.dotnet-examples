
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

builder.Services.AddScoped<WeatherForecastQuery>();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization() // Enable authorization
    .AddQueryType<WeatherForecastQuery>();
    //.AddMutationType<FirebaseMutation>();

builder.Services.AddSingleton(FirebaseApp.Create()); //Adds the firebase credentials
builder.Services.AddFirebaseAuthentication(); //Performs the Auth

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.Run();
