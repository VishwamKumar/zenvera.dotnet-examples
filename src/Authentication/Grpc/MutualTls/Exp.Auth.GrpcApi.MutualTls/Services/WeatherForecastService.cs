namespace Exp.Auth.GrpcApi.MutualTls.Services;

public class WeatherForecastService(ILogger<WeatherForecastService> logger) : WeatherForecaster.WeatherForecasterBase
{

    public override Task<WeatherReply> GetWeatherForecast(WeatherRequest request, ServerCallContext context)
    {
        var rng = new Random();
        var forecasts = Enumerable.Range(1, request.Days).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index).ToString("d"),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)],
            TemperatureF = 32 + (int)(rng.Next(-20, 55) / 0.5556)
        }).ToArray();

        var reply = new WeatherReply();
        reply.Forecasts.AddRange(forecasts);
        logger.LogInformation("Weather Forecast sent for {days}", request.Days);
        return Task.FromResult(reply);
    }

    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
}

