public class SourceTextTest
{
    [Fact]
    public void Constructor_WithValidText_CreatesSourceText()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var documentText = new SourceText(value);

        // Assert
        Assert.Equal(value, documentText.Value);
    }


    [Fact]
    public void Constructor_WithEmptyText_ThrowsArgumentException()
    {
        // Arrange
        var value = "";

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new SourceText(value));
    }

    [Fact]
    public void SourceTexts_WithSameValue_AreEqual()
    {
        // Arrange
        var value = "Hello";

        // Act
        var text1 = new SourceText(value);
        var text2 = new SourceText(value);


        // Assert
        Assert.Equal(text1, text2);
    }
}