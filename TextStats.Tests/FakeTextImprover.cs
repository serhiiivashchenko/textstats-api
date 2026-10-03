public class FakeTextImprover : ITextImprover
{
    public Task<string> ImproveTextAsync(string text)
    {
        return Task.FromResult($"Improved: {text}");
    }
}