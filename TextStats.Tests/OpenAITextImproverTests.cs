using Microsoft.Extensions.Options;

public class OpenAITextImproverTests
{
    [Fact]
    public async Task ImproveTextAsync_WhenCancelledDuringHttpRequest_ThrowsOperationCanceledException()
    {
        // Arrange
        using var blockingHttpMessageHandler = new BlockingHttpMessageHandler();
        using var httpClient = new HttpClient(blockingHttpMessageHandler)
        {
            BaseAddress = new Uri("https://example.test/")
        };
        var options = Options.Create(new OpenAISettings
        {
            Model = "gpt-5-nano"
        });
        using var cts = new CancellationTokenSource();

        var improver = new OpenAITextImprover(httpClient, options);

        // Act
        var task = improver.ImproveTextAsync(
            "Hello world",
            cts.Token);

        await blockingHttpMessageHandler.RequestStarted.Task
            .WaitAsync(TimeSpan.FromSeconds(5));

        cts.Cancel();

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => task.WaitAsync(TimeSpan.FromSeconds(3)));
    }
}


