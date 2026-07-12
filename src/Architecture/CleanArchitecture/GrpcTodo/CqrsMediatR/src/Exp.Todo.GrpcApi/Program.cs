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
    // Use proper logging instead of Console.WriteLine
    var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogCritical(ex, "Application failed to start");
    throw;
}
