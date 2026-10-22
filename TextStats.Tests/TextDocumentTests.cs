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
}