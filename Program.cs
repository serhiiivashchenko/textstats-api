var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ITextAnalyzer, TextAnalyzer>();

var app = builder.Build();

app.MapPost("/api/analyze", (
    AnalyzeRequest request,
    ITextAnalyzer analyzer) =>
{
    return analyzer.Analyze(request.Text);

});

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "TextStats.Api"

}));

app.Run();

record AnalyzeRequest(string Text);

public record AnalyzeResponse(
    int Characters,
    int Words
);