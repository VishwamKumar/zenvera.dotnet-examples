namespace Exp.Weather.RestApi.RedisCaching.Dtos;

public class GetWeatherForecastResponse
{
    public string City { get; set; } = null!;
    public DateTime ForecastTime { get; set; }
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; } = [];

}
