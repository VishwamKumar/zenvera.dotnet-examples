namespace Zenvera.DotNetExamples.Reference.BlazorServerHost;

public sealed class NotesApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<NoteDto>> GetNotesAsync(CancellationToken cancellationToken = default)
    {
        var notes = await httpClient.GetFromJsonAsync<List<NoteDto>>("/notes", cancellationToken);
        return notes ?? [];
    }
}

public sealed record NoteDto(int Id, string Title, DateTimeOffset CreatedUtc);
