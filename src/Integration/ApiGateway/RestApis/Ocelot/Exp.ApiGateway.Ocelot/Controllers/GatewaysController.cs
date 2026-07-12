namespace Exp.ApiGateway.Ocelot.Controllers;

[ApiController]
[Route("[controller]")]
public class GatewaysController : ControllerBase
{
    private readonly ILogger<GatewaysController> _logger;
    public GatewaysController(ILogger<GatewaysController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("CheckStatus")]
    public async Task<string> CheckStatus()
    {
        _logger.LogInformation("StatusCheck was called");
        string response = "API Gateway is running.";
        return await Task.FromResult(response);
    }
}
