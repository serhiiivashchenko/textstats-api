using System.Net;

public class CatchingRequestHttpHandler : HttpMessageHandler
{
    public HttpRequestMessage? CaughtRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CaughtRequest = request;

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }
}