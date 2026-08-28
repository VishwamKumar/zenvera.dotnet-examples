namespace Exp.Shared.Queuing;

internal sealed class RabbitMqService(RabbitMqSettings settings, ILogger<RabbitMqService> logger)
    : IRabbitMqService, IAsyncDisposable, IDisposable
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    public async Task PublishAsync(string message, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        var channel = await EnsureChannelAsync(cancellationToken);
        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(
            exchange: settings.Exchange ?? string.Empty,
            routingKey: settings.RoutingKey ?? string.Empty,
            body: body,
            cancellationToken: cancellationToken);
    }

    public async Task<string?> ConsumeAsync(CancellationToken cancellationToken = default)
    {
        var channel = await EnsureChannelAsync(cancellationToken);
        var result = await channel.BasicGetAsync(settings.ResolvedQueueName, autoAck: true, cancellationToken);
        return result is null ? null : Encoding.UTF8.GetString(result.Body.ToArray());
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }

        _gate.Dispose();
    }

    private async Task<IChannel> EnsureChannelAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_channel is { IsOpen: true })
        {
            return _channel;
        }

        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            var factory = new ConnectionFactory
            {
                HostName = settings.Host,
                Port = settings.Port,
                VirtualHost = string.IsNullOrWhiteSpace(settings.VirtualHost) ? "/" : settings.VirtualHost
            };

            if (settings.AuthEnabled)
            {
                factory.UserName = settings.UserName ?? "guest";
                factory.Password = settings.Password ?? "guest";
            }

            logger.LogInformation("Connecting to RabbitMQ at {Host}:{Port}/{VirtualHost}", factory.HostName, factory.Port, factory.VirtualHost);
            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var queue = settings.ResolvedQueueName;
            if (!string.IsNullOrWhiteSpace(settings.Exchange))
            {
                await _channel.ExchangeDeclareAsync(settings.Exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
                await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
                await _channel.QueueBindAsync(queue, settings.Exchange, settings.RoutingKey ?? string.Empty, cancellationToken: cancellationToken);
            }
            else
            {
                await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            }

            return _channel;
        }
        finally
        {
            _gate.Release();
        }
    }
}
