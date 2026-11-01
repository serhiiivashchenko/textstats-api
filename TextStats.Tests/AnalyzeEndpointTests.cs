using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;

public class AnalyzeEndpointTests
{
    [Theory]
    [InlineData("{\"text\":null}")]
    [InlineData("{}")]
    public async Task Analyze_WithNullOrMissingText_ReturnsBadRequest(string json)
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await client.PostAsync("/api/analyze", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.Equal("Text is required.", problem.Title);
    }

    [Theory]
    [InlineData("", 0, 0, 0)]
    [InlineData("Hello world.", 12, 2, 1)]
    public async Task Analyze_WithText_ReturnsCounts(
        string text, int characters, int words, int sentences)
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/analyze", new { text });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<AnalyzeResponse>();
        Assert.NotNull(result);
        Assert.Equal(new AnalyzeResponse(characters, words, sentences), result);
    }
}
