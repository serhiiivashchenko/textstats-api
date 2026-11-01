public class TextAnalyzerTests
{
    [Theory]
    [InlineData("hello\nworld", 2, 1)]
    [InlineData("hello\tworld\r\nagain", 3, 1)]
    [InlineData("hello\u00a0world", 2, 1)]
    [InlineData("Hello. ", 1, 1)]
    [InlineData("Hello! \n How are you? \t", 4, 2)]
    [InlineData(" \t\r\n", 0, 0)]
    [InlineData("Hello... ! ? World?", 4, 2)]
    public void Analyze_WithWhitespaceAndPunctuation_ReturnsCorrectCounts(
        string text, int expectedWords, int expectedSentences)
    {
        var result = new TextAnalyzer().Analyze(text);

        Assert.Equal(text.Length, result.Characters);
        Assert.Equal(expectedWords, result.Words);
        Assert.Equal(expectedSentences, result.Sentences);
    }

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
