namespace Exp.Weather.RestApi.RabbitMq.Dtos;

public class GetWeatherForecastResponse
{
    public string City { get; set; } = null!;
    public DateTime ForecastTime { get; set; }
    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; } = [];
    public string RabbitMqSendStatus { get; set; } = null!;
    public string RabbitMqReceivedStatus { get; set; } = null!;
}
