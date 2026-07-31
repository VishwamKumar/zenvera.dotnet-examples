namespace Zenvera.DotNetExamples.Reference.Tests;

public sealed class OrchestrationTests(AppHostFixture fixture) : IClassFixture<AppHostFixture>
{
    // The fixture already waited for healthy resources, so requests only need a short budget.
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromMinutes(2);

    [Fact]
    public async Task GetNotesFromApiReturnsSeededNotes()
    {
        using var request = new CancellationTokenSource(RequestTimeout);
        var httpClient = fixture.App.CreateHttpClient("reference-api");

        var response = await httpClient.GetAsync("/notes", request.Token);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var notes = await response.Content.ReadFromJsonAsync<List<NoteDto>>(request.Token);
        Assert.NotNull(notes);
        Assert.NotEmpty(notes);
        Assert.Contains(notes, n => n.Title.Contains("Aspire", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPortalRootReturnsOk()
    {
        using var request = new CancellationTokenSource(RequestTimeout);
        var httpClient = fixture.App.CreateHttpClient("reference-portal");

        var response = await httpClient.GetAsync("/", request.Token);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private sealed record NoteDto(int Id, string Title, DateTimeOffset CreatedUtc);
}
