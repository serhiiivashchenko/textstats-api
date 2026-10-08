public interface ITextImprover
{
    Task<string> ImproveTextAsync(string text, CancellationToken cancellationToken);
}