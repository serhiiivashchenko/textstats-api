using Microsoft.EntityFrameworkCore;

public class EfCoreLearningTests
{

    [Fact]
    public void ModifyTrackedEntity_ChangesStateToModified()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new AppDbContext(options);

        var note = new Note { Text = "Hello EF Core" };

        context.Notes.Add(note);

        Assert.Equal(EntityState.Added, context.Entry(note).State);
    }

    [Fact]
    public void ModifyAddedEntity_ChangesStatetoModified()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new AppDbContext(options);

        var note = new Note { Text = "Hello EF Core" };

        context.Entry(note).State = EntityState.Unchanged;

        note.Text = "Updated text";
        context.ChangeTracker.DetectChanges();


        Assert.Equal(EntityState.Modified, context.Entry(note).State);
    }

    [Fact]
    public async Task SaveChangesAsync_PersistsAddedEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var context = new AppDbContext(options);
        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();

        // Act
        var note = new Note { Text = "Hello EF Core" };
        context.Notes.Add(note);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(EntityState.Unchanged, context.Entry(note).State);
    }
}