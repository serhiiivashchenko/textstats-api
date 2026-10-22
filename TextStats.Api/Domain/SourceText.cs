public sealed record SourceText
{
    public string Value { get; }

    public SourceText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Text cannot be empty.", nameof(value));
        }

        if (value.Length > 2000)
        {
            throw new ArgumentException("Text cannot be longer than 2000 symbols.", nameof(value));
        }
        Value = value;
    }
}