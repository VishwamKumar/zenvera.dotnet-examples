namespace Zenvera.DotNetExamples.Reference.Tests;

public class OrchestrationTests
{
    // SQL Server container startup routinely needs more than the template default.
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(3);

    [Fact]
    public async Task GetNotesFromApiReturnsSeededNotes()
    {
        var cancellationToken = new CancellationTokenSource(DefaultTimeout).Token;

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

        await using var app = await appHost.BuildAsync(cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);
        await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        var httpClient = app.CreateHttpClient("reference-api");
        await app.ResourceNotifications
            .WaitForResourceHealthyAsync("reference-api", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        var response = await httpClient.GetAsync("/notes", cancellationToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var notes = await response.Content.ReadFromJsonAsync<List<NoteDto>>(cancellationToken);
        Assert.NotNull(notes);
        Assert.NotEmpty(notes);
        Assert.Contains(notes, n => n.Title.Contains("Aspire", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPortalRootReturnsOk()
    {
        var cancellationToken = new CancellationTokenSource(DefaultTimeout).Token;

        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Zenvera_DotNetExamples_Reference_AppHost>(cancellationToken);

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync(cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);
        await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        var httpClient = app.CreateHttpClient("reference-portal");
        await app.ResourceNotifications
            .WaitForResourceHealthyAsync("reference-portal", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        var response = await httpClient.GetAsync("/", cancellationToken);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed record NoteDto(int Id, string Title, DateTimeOffset CreatedUtc);
}
