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
    Log.Fatal(ex, "Application failed to start");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

