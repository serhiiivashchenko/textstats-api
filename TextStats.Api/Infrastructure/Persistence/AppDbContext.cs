using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<TextDocument> TextDocuments => Set<TextDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TextDocumentConfiguration());
    }
}

public class Note
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}