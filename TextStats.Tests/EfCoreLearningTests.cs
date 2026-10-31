using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microsoft.Extensions.Configuration;

public class EfCoreLearningTests
{

    [Fact]
    public void AddingEntity_ChangesStateToAdded()
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
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        // Act
        var note = new Note { Text = "Hello EF Core" };
        context.Notes.Add(note);
        await context.SaveChangesAsync();

        using var readContext = new AppDbContext(options);

        var savedNote = await readContext.Notes
            .AsNoTracking()
            .SingleAsync();

        // Assert
        Assert.Equal(EntityState.Unchanged, context.Entry(note).State);
        Assert.True(note.Id > 0);
        Assert.Equal(note.Id, savedNote.Id);
        Assert.Equal(note.Text, savedNote.Text);
    }

    [Fact]
    public async Task UpdateTrackedEntity_PersistsChanges()
    {
        // Arrange
        var newText = "Updated text!";

        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .Options;

        using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var note = new Note { Text = "Original text" };
        context.Notes.Add(note);
        await context.SaveChangesAsync();

        // Act
        note.Text = newText;
        await context.SaveChangesAsync();

        using var readContext = new AppDbContext(options);
        var savedNote = await readContext.Notes
            .AsNoTracking()
            .SingleAsync();

        // Assert
        Assert.Equal(EntityState.Unchanged, context.Entry(note).State);
        Assert.Equal(newText, savedNote.Text);
    }

    [Fact]
    public async Task TextDocument_CanBePersistedAndRestored()
    {
        var textToImprove = "Hello EF Core!";
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var document = new TextDocument(new SourceText(textToImprove));

        using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        context.TextDocuments.Add(document);
        await context.SaveChangesAsync();

        using var readContext = new AppDbContext(options);
        var savedTextDocument = await readContext.TextDocuments
            .AsNoTracking()
            .SingleAsync();

        Assert.Equal(textToImprove, savedTextDocument.OriginalText.Value);
        Assert.Equal(document.Id, savedTextDocument.Id);
        Assert.Null(savedTextDocument.ImprovedText);
        Assert.Equal(DocumentStatus.Draft, savedTextDocument.Status);
    }
}