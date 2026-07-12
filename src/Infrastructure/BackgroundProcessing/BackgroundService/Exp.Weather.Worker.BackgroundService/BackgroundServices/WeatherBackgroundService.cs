namespace Exp.Weather.Worker.BackgroundService.BackgroundServices;

//IHttpClientFactory httpClientFactory
public class WeatherBackgroundService(ILogger<WeatherBackgroundService> logger,
    IConfiguration configuration) : Microsoft.Extensions.Hosting.BackgroundService
{
    private IEnumerable<WeatherForecastDto>? latestWeather;
        
    public IEnumerable<WeatherForecastDto>? GetLatestWeather() => latestWeather;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int freqInMin = int.Parse(configuration.GetSection("BackgroudServiceSettings:PollingInMinutes").Value??"1");
        while (!stoppingToken.IsCancellationRequested)
        {
            latestWeather = await FetchWeatherDataAsync();
            logger.LogInformation("Weather data updated at {date}", DateTime.Now);
            await Task.Delay(TimeSpan.FromMinutes(freqInMin), stoppingToken);
        }
    }

    private static async Task<IEnumerable<WeatherForecastDto>> FetchWeatherDataAsync()
    {
        //Following code can pull data from other API in background
        //var httpClient = httpClientFactory.CreateClient();
        //var response = await httpClient.GetStringAsync("https://api.weatherapi.com/v1/current.json?key=YOUR_API_KEY&q=London");
        //var weatherJson = JObject.Parse(response);

        //return new WeatherForecastDto
        //{  
        //    TemperatureC = weatherJson["current"]["temp_c"].ToObject<double>(),
        //    Summary = weatherJson["current"]["condition"]["text"].ToString(),
        //    Date = DateOnly.FromDateTime(DateTime.Now)
        //};
        await Task.Delay(1);
        return WeatherService.GetWeatherInfo();
    }
}
