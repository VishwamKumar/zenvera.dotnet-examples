namespace Zenvera.DotNetExamples.Reference.RestApiHost.Data;

public sealed class ReferenceDbContext(DbContextOptions<ReferenceDbContext> options) : DbContext(options)
{
    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Notes");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
        });
    }
}
