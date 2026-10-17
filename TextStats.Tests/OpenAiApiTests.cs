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

        using var httpClient = new HttpClient(catchingRequestHttpHandler);
        httpClient.BaseAddress = requestUri;

        var api = RestService.For<IOpenAiApi>(httpClient);

        var body = new OpenAiRequestBody(Model: model, Instructions: instruction, Input: input);

        using var response = await api.CreateResponseAsync(body, CancellationToken.None);

        var json = Assert.IsType<string>(catchingRequestHttpHandler.CaughtRequestBody);
        var actualBody = JsonSerializer.Deserialize<OpenAiRequestBody>(json);

        var caughtRequest = Assert.IsType<HttpRequestMessage>(catchingRequestHttpHandler.CaughtRequest);
        Assert.Equal(HttpMethod.Post, catchingRequestHttpHandler.CaughtRequest.Method);

        Assert.Equal(new Uri(requestUri, "responses"), catchingRequestHttpHandler.CaughtRequest.RequestUri);
        Assert.Equal(body, actualBody);

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal(model, root.GetProperty("model").GetString());
        Assert.Equal(instruction, root.GetProperty("instructions").GetString());
        Assert.Equal(input, root.GetProperty("input").GetString());
    }
}