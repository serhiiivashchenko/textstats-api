using System.Text.Json;
using Refit;

public class OpenAiApiTests
{
    private static readonly Uri requestUri = new("https://example.test/");
    private static readonly string model = "test-model-io";
    private static readonly string instruction = "test-instruction";
    private static readonly string input = "test-input";

    [Fact]
    public async Task CreateResponseAsync_WhenCalled_SendsExpectedHttpRequest()
    {
        // Arrange
        using var catchingRequestHttpHandler = new CatchingRequestHttpHandler();
        using var httpClient = new HttpClient(catchingRequestHttpHandler)
        {
            BaseAddress = requestUri
        };

        var api = RestService.For<IOpenAiApi>(httpClient);

        var body = new OpenAiRequestBody(Model: model, Instructions: instruction, Input: input);

        using var response = await api.CreateResponseAsync(body, CancellationToken.None);
        var json = await catchingRequestHttpHandler.CaughtRequest.Content.ReadAsStringAsync();

        var actualBody = JsonSerializer.Deserialize<OpenAiRequestBody>(json);


        Assert.Equal(HttpMethod.Post, catchingRequestHttpHandler.CaughtRequest.Method);
        Assert.Equal(requestUri.AbsolutePath + "responses", catchingRequestHttpHandler.CaughtRequest.RequestUri.AbsoluteUri);
        Assert.Equal(body, actualBody);
    }
}