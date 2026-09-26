public class TextAnalyzer : ITextAnalyzer
{
    public AnalyzeResponse Analyze(string text)
    {
        var words = text
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Length;

        return new AnalyzeResponse(
            Characters: text.Length,
            Words: words
        );
    }
}