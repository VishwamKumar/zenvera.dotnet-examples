namespace Zenvera.DotNetExamples.Reference.RestApiHost.Data;

public sealed class Note
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset CreatedUtc { get; set; }
}
