namespace Zenvera.DotNetExamples.Reference.Tests;

/// <summary>
/// Starts the AppHost topology once for the whole test class and waits until the
/// product hosts report healthy.
/// </summary>
public sealed class AppHostFixture : IAsyncLifetime
{
    // A cold agent still has to run SQL Server first-start initialisation before the
    // API can become healthy, so the startup budget stays well above a warm local run.
    private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(8);

    private readonly ConcurrentDictionary<string, string> _resourceStates = new(StringComparer.Ordinal);

    private DistributedApplication? _app;

    public DistributedApplication App =>
        _app ?? throw new InvalidOperationException("The AppHost topology has not been started.");

    public async ValueTask InitializeAsync()
    {
        using var startup = new CancellationTokenSource(StartupTimeout);
        var cancellationToken = startup.Token;

        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Zenvera_DotNetExamples_Reference_AppHost>(cancellationToken);

        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Information);
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Information);
        });

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _app = await appHost.BuildAsync(cancellationToken);

        // Test host logs are not surfaced by the runner, so the observed resource states are
        // the only way a CI-only stall can be told apart from a plain timeout. The watch runs
        // until startup settles, so it needs a trigger of its own.
        using var watchUntilStarted = new CancellationTokenSource();
        var watch = WatchResourceStatesAsync(_app, _resourceStates, watchUntilStarted.Token);

        try
        {
            await _app.StartAsync(cancellationToken);
            await _app.ResourceNotifications.WaitForResourceHealthyAsync("reference-api", cancellationToken);
            await _app.ResourceNotifications.WaitForResourceHealthyAsync("reference-portal", cancellationToken);
        }
        catch (OperationCanceledException) when (startup.IsCancellationRequested)
        {
            throw new TimeoutException(
                $"The AppHost topology did not become healthy within {StartupTimeout}. {DescribeResourceStates()}");
        }
        finally
        {
            await watchUntilStarted.CancelAsync();
            await watch;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }

    private static async Task WatchResourceStatesAsync(
        DistributedApplication app,
        ConcurrentDictionary<string, string> states,
        CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var change in app.ResourceNotifications.WatchAsync(cancellationToken))
            {
                var state = change.Snapshot.State?.Text ?? "unknown";
                var health = change.Snapshot.HealthStatus?.ToString() ?? "no health report";
                states[change.Resource.Name] = $"{state} ({health})";
            }
        }
        catch (OperationCanceledException)
        {
            // Expected once the startup budget expires or the topology is torn down.
        }
    }

    private string DescribeResourceStates()
    {
        if (_resourceStates.IsEmpty)
        {
            return "No resource reported any state, which points at the orchestrator rather than the sample.";
        }

        var states = _resourceStates
            .OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => $"{pair.Key}: {pair.Value}");

        return $"Last observed resource states: {string.Join("; ", states)}.";
    }
}
