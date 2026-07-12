namespace Exp.Weather.RestApi.Serilog.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddCustomLogging();
        //builder.Services.AddLogTypeLogger(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}
