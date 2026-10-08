using System.Net;

public class BlockingHttpMessageHandler : HttpMessageHandler
{
    public TaskCompletionSource RequestStarted { get; } = new(
        TaskCreationOptions.RunContinuationsAsynchronously);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        RequestStarted.SetResult();

        await Task.Delay(
            Timeout.InfiniteTimeSpan,
            cancellationToken);

        return new HttpResponseMessage(HttpStatusCode.OK);
    }
}