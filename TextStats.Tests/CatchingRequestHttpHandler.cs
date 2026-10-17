using System.Net;

public class CatchingRequestHttpHandler : HttpMessageHandler
{
    public HttpRequestMessage? CaughtRequest { get; private set; }
    public string? CaughtRequestBody { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CaughtRequest = request;
        CaughtRequestBody = request.Content is null
        ? null
        : await request.Content.ReadAsStringAsync(cancellationToken);

        return new HttpResponseMessage(HttpStatusCode.OK);
    }
}