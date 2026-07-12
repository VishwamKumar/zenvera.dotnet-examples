var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = true;
    });
      
builder.Services.AddGrpcReflection(); //Helps Postman to find services
builder.Services.AddControllers();

builder.Services.AddLogging();
builder.Services.Configure<List<UserCredential>>(builder.Configuration.GetSection("UserCredentials"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings!);
builder.Services.AddSingleton<JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting(); //Must be first before any middleware in this case
//Handles all the exceptions
app.UseMiddleware<ExceptionMiddleware>();
// Add JWT middleware for validating
app.UseMiddleware<JwtAuthMiddleware>();

app.MapGrpcService<WeatherForecastService>();
app.MapGrpcReflectionService();
app.MapControllers();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
