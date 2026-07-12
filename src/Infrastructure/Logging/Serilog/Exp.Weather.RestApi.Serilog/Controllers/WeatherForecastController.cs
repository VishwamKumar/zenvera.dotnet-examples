
namespace Exp.Weather.RestApi.Serilog.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogTypeLogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
   [
       "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        try
        {
            string testEmail = "vishwa.kumar@slchq.com";
            string testSSN = "234-02-6987";
            string testCard = "1234-5678-9012-3456";
            logger.Info(LogType.Application, "Method: {Method}, Email: {Email}, SSN: {SSN}, Card: {Card} - Get Weather Info", Request.Method, testEmail, testSSN, testCard);

            var response = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            logger.Info(LogType.Transaction, "Method: {Method}, Email: {Email}, SSN: {SSN}, Card: {Card}  - Response Body: {@Response}", Request.Method, testEmail, testSSN, testCard, response);

            return response;
        }
        catch (Exception ex)
        {
            logger.Error(LogType.Application, ex, "Method: {Method} - Error occurred: {ErrorMessage}", Request.Method, ex.Message);
            throw;
        }
    }
}

