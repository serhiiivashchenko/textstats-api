public class TextAnalyzer : ITextAnalyzer
{
    public AnalyzeResponse Analyze(string text)
    {
        var words = text
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .Length;
        
        var sentences = text
            .Split(['.', '!', '?'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Length;

        return new AnalyzeResponse(
            Characters: text.Length,
            Words: words,
            Sentences:sentences
        );
    }
}
