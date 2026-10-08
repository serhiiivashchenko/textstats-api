public class FakeTextImprover : ITextImprover
{
    public Task<string> ImproveTextAsync(string text, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Improved: {text}");
    }
}