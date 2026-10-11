public class OpenAISettings
{
    public string BaseAddress { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
    public string ApiKey { get; set; } = string.Empty;
}