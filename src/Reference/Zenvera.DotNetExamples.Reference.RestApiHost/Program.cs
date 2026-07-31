var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<ReferenceDbContext>("reference-db");
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ReferenceDbContext>();
    await ReferenceDbInitializer.InitializeAsync(db);
}

app.MapGet("/", () => Results.Ok(new
{
    service = "reference-api",
    message = "Zenvera Aspire reference REST API. Use GET /notes."
}));

app.MapGet("/notes", async (ReferenceDbContext db, CancellationToken cancellationToken) =>
{
    var notes = await db.Notes
        .OrderBy(n => n.Id)
        .Select(n => new NoteDto(n.Id, n.Title, n.CreatedUtc))
        .ToListAsync(cancellationToken);

    return Results.Ok(notes);
})
.WithName("GetNotes");

app.MapDefaultEndpoints();
app.Run();

internal sealed record NoteDto(int Id, string Title, DateTimeOffset CreatedUtc);
