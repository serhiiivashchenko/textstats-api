using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

public class ImproveEndpointTests
{
    [Fact]
    public async Task Improve_ReturnsOk()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/improve",
            new
            {
                text = "I has experience."
            });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var serviceResponse = await response.Content.ReadFromJsonAsync<ImproveResponse>();
        Assert.NotNull(serviceResponse);
        Assert.Equal("I has experience.", serviceResponse.Original);
        Assert.Equal("Improved: I has experience.", serviceResponse.Improved);
    }

    [Fact]
    public async Task Improve_WithEmptyText_ReturnsBadRequest()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/improve",
            new
            {
                text = ""
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Improve_WithTextOver2000Characters_ReturnsBadRequest()
    {
        await using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/improve",
            new
            {
                text = new string('a', 2001)
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Improve_WhenImproverFails_ReturnsBadGateway()
    {
        var expectedTitle = "Text improvement service is temporarily unavailable.";

        await using var factory = new CustomWebApplicationFactory();

        var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<ITextImprover, FailingTextImprover>();
                });
            })
            .CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/improve",
            new
            {
                text = "Hello"
            });

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var serviceResponse = await response.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ProblemDetails>();
        Assert.NotNull(serviceResponse);

        Assert.Equal(expectedTitle, serviceResponse.Title);
        Assert.Equal((int)HttpStatusCode.BadGateway, serviceResponse.Status);
    }
}

public record ImproveResponse(
    string Original,
    string Improved);