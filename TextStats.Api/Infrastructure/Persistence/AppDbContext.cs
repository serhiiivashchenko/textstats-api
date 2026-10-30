using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<Note> Notes => Set<Note>();
}

public class Note
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}