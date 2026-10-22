public class TextDocumentTests
{
    [Fact]
    public void Constructor_CreatesUniqueIdForEachDocument()
    {
        // Arrange
        var sourceText = new SourceText(value: "Hello!");

        // Act
        var textDocument1 = new TextDocument(sourceText);
        var textDocument2 = new TextDocument(sourceText);

        Assert.NotEqual(textDocument1.Id, textDocument2.Id);
    }

    [Fact]
    public void Constructor_CreatesDocumentInDraftStatus()
    {
        // Arrange
        var sourceText = new SourceText(value: "Hello World!");

        // Act
        var textDocument = new TextDocument(sourceText);

        // Assert
        Assert.Equal(DocumentStatus.Draft, textDocument.Status);
    }

    [Fact]
    public void Improve_WithValidText_ChangesDocumentStatusToImproved()
    {
        // Arrange
        var improvedText = "Improved Text!";
        var sourceText = new SourceText(value: "Hello World!");
        var textDocument = new TextDocument(sourceText);

        // Act
        textDocument.Improve(improvedText);

        // Assert
        Assert.Equal(DocumentStatus.Improved, textDocument.Status);
        Assert.Equal(improvedText, textDocument.ImprovedText);
    }

    [Fact]
    public void Improve_WhenDocumentAlreadyImproved_ThrowsInvalidOperationException()
    {
        // Arrange
        var improvedText = "Improved Text!";
        var sourceText = new SourceText(value: "Hello World!");
        var textDocument = new TextDocument(sourceText);
        textDocument.Improve(improvedText);

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => textDocument.Improve("Second improvement"));
        Assert.Equal(DocumentStatus.Improved, textDocument.Status);
        Assert.Equal(improvedText, textDocument.ImprovedText);
    }
}