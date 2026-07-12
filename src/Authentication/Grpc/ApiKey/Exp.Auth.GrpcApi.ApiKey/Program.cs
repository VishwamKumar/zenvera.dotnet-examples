var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = true;
    });
      
builder.Services.AddGrpcReflection(); //Helps Postman to find services
builder.Services.AddLogging();
builder.Services.Configure<List<ApiKeyCredential>>(builder.Configuration.GetSection("ApiKeys"));
var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();
// Add API Key middleware before routing
app.UseMiddleware<ApiKeyAuthMiddleware>();

app.MapGrpcService<WeatherForecastService>();
app.MapGrpcReflectionService();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
