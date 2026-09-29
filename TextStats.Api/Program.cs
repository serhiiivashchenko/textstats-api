var builder = WebApplication.CreateBuilder(args);

var textStatsApiKey = builder.Configuration["TEXTSTATS_API_KEY"]
    ?? throw new InvalidOperationException("TEXTSTATS_API_KEY is not configured.");

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

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/analyze", (
    AnalyzeRequest request,
    ITextAnalyzer analyzer) =>
{
    return analyzer.Analyze(request.Text);

});

app.MapPost("/api/improve", async (
    HttpRequest httpRequest,
    ImproveRequest request,
    ITextImprover improver) =>
{
    var providedApiKey = httpRequest.Headers["X-Api-Key"].FirstOrDefault();
    if (providedApiKey != textStatsApiKey) return Results.Unauthorized();

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
});

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