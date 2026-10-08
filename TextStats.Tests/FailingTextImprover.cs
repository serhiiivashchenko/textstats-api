public class FailingTextImprover : ITextImprover
{
    public Task<string> ImproveTextAsync(string text, CancellationToken cancellationToken)
    {
        throw new ExternalServiceException("OpenAI failed");
    }
}