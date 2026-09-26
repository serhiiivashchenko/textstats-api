public class TextAnalyzerTests
{
    [Fact]
    public void Analyze_ReturnsCorrectWordCount()
    {
        // Arrange
        var analyzer = new TextAnalyzer();
        var text = "Hello world from C sharp";

        // Act
        var result = analyzer.Analyze(text);

        // Assert
        Assert.Equal(5, result.Words);
    }

    [Fact]
    public void Analyze_ReturnsCorrectSentenceCount()
    {
        // Arrange
        var analyzer = new TextAnalyzer();
        var text = "Hello world. How are you? I am fine!";

        // Act
        var result = analyzer.Analyze(text);

        // Assert
        Assert.Equal(3, result.Sentences);
    }

    [Fact]
    public void Analyze_WithMultipleSpaces_ReturnsCorrectWordCount()
    {
        // Arrange
        var analyzer = new TextAnalyzer();
        var text = "Hello    world";

        // Act
        var result = analyzer.Analyze(text);

        // Assert
        Assert.Equal(2, result.Words);
    }

    [Fact]
    public void Analyze_WithEmptyText_ReturnsZeroCounts()
    {
        // Arrange
        var analyzer = new TextAnalyzer();
        var text = "";

        // Act
        var result = analyzer.Analyze(text);

        // Assert
        Assert.Equal(0, result.Characters);
        Assert.Equal(0, result.Words);
        Assert.Equal(0, result.Sentences);
    }
}