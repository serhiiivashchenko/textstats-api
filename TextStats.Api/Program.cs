using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddPolicy("improve", httpContext =>
    {
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ipAddress,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
    });

builder.Services.AddScoped<ITextAnalyzer, TextAnalyzer>();

builder.Services.AddHttpClient<ITextImprover, OpenAITextImprover>(client =>
{
    var apiKey = builder.Configuration["OPENAI_API_KEY"]
        ?? throw new InvalidOperationException("OPENAI_API_KEY is not configured.");

    client.BaseAddress = new Uri("https://api.openai.com/v1/");
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
});

var app = builder.Build();
app.UseRateLimiter();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/analyze", (
    AnalyzeRequest request,
    ITextAnalyzer analyzer) =>
{
    return analyzer.Analyze(request.Text);

});

app.MapPost("/api/improve", async (
    ImproveRequest request,
    ITextImprover improver) =>
{
    if (string.IsNullOrWhiteSpace(request.Text))

    {
        return Results.BadRequest(new
        {
            error = "Text must not be empty."
        });
    }
    if (request.Text.Length > 2000)
    {
        return Results.BadRequest(new
        {
            error = $"Text must not exceed 2000 characters. Received: {request.Text.Length}."
        });
    }

    try
    {
        var improved = await improver.ImproveTextAsync(request.Text);
        return Results.Ok(new
        {
            original = request.Text,
            improved
        });
    }
    catch (ExternalServiceException)
    {
        return Results.Problem(
            statusCode: StatusCodes.Status502BadGateway,
            title: "Text improvement service is temporarily unavailable."
        );
    }
}).RequireRateLimiting("improve");

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "TextStats.Api"

}));

app.Run();

record AnalyzeRequest(string Text);
record ImproveRequest(string Text);

public record AnalyzeResponse(
    int Characters,
    int Words,
    int Sentences
);