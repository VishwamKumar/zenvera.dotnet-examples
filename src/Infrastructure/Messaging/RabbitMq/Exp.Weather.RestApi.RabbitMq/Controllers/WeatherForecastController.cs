
namespace Exp.Weather.RestApi.RabbitMq.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IRabbitMqService rabbitMqService,
    ILogger<WeatherForecastController> logger) : ControllerBase
{  
    [HttpGet("{city}")]
    public async Task<GetWeatherForecastResponse> Get(string city)
    {
        IEnumerable<WeatherForecast> weatherForecastData= GetWeatherForecastData();
        GetWeatherForecastResponse weatherForecastRespData = new()
                        {
                            City = city,
                            ForecastTime = DateTime.Now,
                            WeatherForecasts = weatherForecastData
                        };

        // Add  weather data in Q
        string jsonString = JsonSerializer.Serialize(weatherForecastRespData);
        await rabbitMqService.PublishAsync(jsonString);
        weatherForecastRespData.RabbitMqSendStatus = "Message Sent to RabbitMq";

        //Read the Message From Q
        var message = await rabbitMqService.ConsumeAsync();
        weatherForecastRespData.RabbitMqReceivedStatus = $"Message Received {message}";
        return weatherForecastRespData;
    }

    private WeatherForecast[] GetWeatherForecastData()
    {
        try
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError("Sorry! Error occured: {error}", ex.Message);
            throw;
        }
    }


    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
}

