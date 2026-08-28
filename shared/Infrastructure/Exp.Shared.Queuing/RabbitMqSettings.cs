namespace Exp.Shared.Queuing;

public sealed class RabbitMqSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public bool AuthEnabled { get; set; } = true;
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string VirtualHost { get; set; } = "/";
    public string Exchange { get; set; } = "";
    public string RoutingKey { get; set; } = "";
    public string Queue { get; set; } = "";

    public string ResolvedQueueName =>
        string.IsNullOrWhiteSpace(Queue)
            ? (string.IsNullOrWhiteSpace(RoutingKey) ? "weather" : RoutingKey)
            : Queue;
}
