using System.Text.Json;
public class OpenAITextImprover : ITextImprover

{
    private readonly HttpClient _httpClient; 

    public OpenAITextImprover(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> ImproveTextAsync(string text)
    {
        var request = new
        {
            model = "gpt-5-nano",
            instructions = "Improve the writing. Return only the improved text.",
            input = text
        };

        var response = await _httpClient.PostAsJsonAsync("responses", request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalServiceException($"OpenAI returned {(int)response.StatusCode}");
        }
        using var json = JsonDocument.Parse(responseBody);

        foreach (var outputItem in json.RootElement.GetProperty("output").EnumerateArray())
        {
            if (outputItem.GetProperty("type").GetString() != "message")
                continue;

            foreach (var contentItem in outputItem.GetProperty("content").EnumerateArray())
            {
                if (contentItem.GetProperty("type").GetString() != "output_text")
                    continue;

                var improvedText = contentItem.GetProperty("text").GetString();

                if (!string.IsNullOrWhiteSpace(improvedText))
                    return improvedText;
            }
        }

        throw new InvalidOperationException(
            "OpenAI response did not contain output text.");

    }
}