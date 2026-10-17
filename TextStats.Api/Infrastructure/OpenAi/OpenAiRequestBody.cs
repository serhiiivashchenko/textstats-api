using System.Text.Json.Serialization;

public record OpenAiRequestBody(
    [property: JsonPropertyName("model")]
    string Model,
    [property: JsonPropertyName("instructions")]
    string Instructions,
    [property: JsonPropertyName("input")]
    string Input);