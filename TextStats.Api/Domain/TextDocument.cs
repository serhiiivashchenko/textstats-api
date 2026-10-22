public sealed class TextDocument
{
    public Guid Id { get; private set; }
    public SourceText OriginalText { get; private set; }

    public TextDocument(SourceText originalText)
    {
        ArgumentNullException.ThrowIfNull(originalText);
        OriginalText = originalText;
        Id = Guid.NewGuid();
    }
}