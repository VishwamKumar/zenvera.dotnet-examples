var builder = WebApplication.CreateBuilder(args);
                           
try
{
    Log.ForContext("LogType", "Application").Information("Application started.");
    builder.ConfigureRazorServices();
    var app = builder.Build();
    app.ConfigureRequestPipeline();
    app.Run();
}
catch (Exception ex)
{
    Log.ForContext("LogType", "Application").Fatal(ex, "Unhandled exception occurred.");
    Console.WriteLine($"Error setting configs: {ex.Message}");
}
finally
{
    Log.ForContext("LogType", "Application").Information("Application shutting down...");
    Log.CloseAndFlush();
}