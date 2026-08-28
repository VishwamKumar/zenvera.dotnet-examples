namespace Exp.Shared.Queuing;

public interface IRabbitMqService
{
    Task PublishAsync(string message, CancellationToken cancellationToken = default);

    Task<string?> ConsumeAsync(CancellationToken cancellationToken = default);
}
