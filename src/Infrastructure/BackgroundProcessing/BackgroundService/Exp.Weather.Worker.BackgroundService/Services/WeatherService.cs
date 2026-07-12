namespace Exp.Weather.Worker.BackgroundService.Services;

public class WeatherService
{
    public static IEnumerable<WeatherForecastDto> GetWeatherInfo()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
      .ToArray();

    }
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
}
