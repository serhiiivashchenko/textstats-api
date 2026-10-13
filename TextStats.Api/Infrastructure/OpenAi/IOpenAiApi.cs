using Refit;

public interface IOpenAiApi
{
    [Post("/responses")]
    Task<HttpResponseMessage> CreateResponseAsync([Body] OpenAiRequestBody body, CancellationToken cancellationToken);
}