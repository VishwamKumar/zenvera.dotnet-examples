
namespace Exp.Auth.RestApi.OAuth2Duende.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];


    [HttpGet(Name = "GetWeatherForecast")]
    //[Authorize(Policy = "weather_read")]
    public IEnumerable<WeatherForecastDto> Get()
    {
        try
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError("Error occured: {ErrorMessage} ",  ex.Message);
            throw;
        }
    }
}

