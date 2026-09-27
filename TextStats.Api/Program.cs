var builder = WebApplication.CreateBuilder(args);
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

app.MapPost("/api/analyze", (
    AnalyzeRequest request,
    ITextAnalyzer analyzer) =>
{
    return analyzer.Analyze(request.Text);

});

app.MapPost("/api/improve", async (
    HttpRequest httpRequest,
    ImproveRequest request,
    ITextImprover improver,
    IConfiguration configuration) =>
{
    var expectedApiKey = configuration["TEXTSTATS_API_KEY"];
    var providedApiKey = httpRequest.Headers["X-Api-Key"].FirstOrDefault();

    if(providedApiKey!=expectedApiKey) return Results.Unauthorized();
   
    var improved = await improver.ImproveTextAsync(request.Text);
    return Results.Ok(new
        {
            original = request.Text,
            improved
        });
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