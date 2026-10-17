using MediatR;
using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<OpenAISettings>()
    .Bind(builder.Configuration.GetSection("OpenAI"))
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.Model), "OpenAI model must be configured.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.ApiKey), "OpenAI ApiKey must be configured.")
        .Validate(
            settings => Uri.TryCreate(settings.BaseAddress, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps,
            "OpenAI BaseAddress must be a valid HTTPS URL.")
        .Validate(
            settings => settings.TimeoutSeconds > 0 && settings.TimeoutSeconds <= 60, "Timeout must be configured and not exceed 60 seconds")
    .ValidateOnStart();

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
builder.Services.AddScoped<ITextImprover, OpenAITextImprover>();

builder.Services
    .AddRefitGeneratedClient<IOpenAiApi>()
    .ConfigureHttpClient(
        (serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<OpenAISettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
        });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseRateLimiter();
app.UseExceptionHandler();

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
    IMediator mediator) =>
    {
        var improved = await mediator.Send(new ImproveTextCommand(request.Text));
        return Results.Ok(new
        {
            original = request.Text,
            improved
        });

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