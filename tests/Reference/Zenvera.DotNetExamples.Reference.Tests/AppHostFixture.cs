namespace Zenvera.DotNetExamples.Reference.Tests;

/// <summary>
/// Starts the AppHost topology once for the whole test class and waits until the
/// product hosts report healthy.
/// </summary>
public sealed class AppHostFixture : IAsyncLifetime
{
    // A cold agent must pull the SQL Server image and run first-start initialisation
    // before the API can become healthy, so the startup budget is deliberately large.
    private static readonly TimeSpan StartupTimeout = TimeSpan.FromMinutes(15);

    private DistributedApplication? _app;

    public DistributedApplication App =>
        _app ?? throw new InvalidOperationException("The AppHost topology has not been started.");

    public async Task InitializeAsync()
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
        await _app.StartAsync(cancellationToken);

        await _app.ResourceNotifications.WaitForResourceHealthyAsync("reference-api", cancellationToken);
        await _app.ResourceNotifications.WaitForResourceHealthyAsync("reference-portal", cancellationToken);
    }

    public async Task DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }
}
