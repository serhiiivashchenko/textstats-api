public class FailingTextImprover : ITextImprover
{
    public Task<string> ImproveTextAsync(string text)
    {
        throw new ExternalServiceException("OpenAI failed");
    }
}