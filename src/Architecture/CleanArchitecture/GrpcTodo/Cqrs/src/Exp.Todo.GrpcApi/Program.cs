
var builder = WebApplication.CreateBuilder(args);

try
{    
    builder.ConfigureServices();
    builder.ConfigureSwaggerServices();

    var app = builder.Build();

    app.ConfigureMiddleware();
    app.ConfigureEndpoints();

    app.Run();
}
catch (Exception ex)
{
  
    Console.WriteLine($"Error setting configs: {ex.Message}");
}
