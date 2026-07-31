namespace Zenvera.DotNetExamples.Reference.RestApiHost.Data;

public static class ReferenceDbInitializer
{
    public static async Task InitializeAsync(ReferenceDbContext db, CancellationToken cancellationToken = default)
    {
        await db.Database.EnsureCreatedAsync(cancellationToken);

        if (await db.Notes.AnyAsync(cancellationToken))
        {
            return;
        }

        db.Notes.AddRange(
            new Note { Title = "Welcome to the Aspire reference sample", CreatedUtc = DateTimeOffset.UtcNow },
            new Note { Title = "SQL starts through AppHost resource references", CreatedUtc = DateTimeOffset.UtcNow },
            new Note { Title = "Blazor calls the API using service discovery", CreatedUtc = DateTimeOffset.UtcNow });

        await db.SaveChangesAsync(cancellationToken);
    }
}
