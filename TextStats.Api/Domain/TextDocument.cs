public sealed class TextDocument
{
    public Guid Id { get; private set; }
    public SourceText OriginalText { get; private set; }
    public DocumentStatus Status { get; private set; }
    public string? ImprovedText { get; private set; }

    private TextDocument()
    {
        OriginalText = null!;
    }
    public TextDocument(SourceText originalText)
    {
        ArgumentNullException.ThrowIfNull(originalText);
        OriginalText = originalText;
        Id = Guid.NewGuid();
        Status = DocumentStatus.Draft;
    }

    public void Improve(string improvedText)
    {
        if (string.IsNullOrWhiteSpace(improvedText))
        {
            throw new ArgumentException($"Improved text can't be empty.");
        }
        if (Status != DocumentStatus.Draft)
        {
            throw new InvalidOperationException($"Document should be in Draft status, but was in {Status}.");
        }
        ImprovedText = improvedText;
        Status = DocumentStatus.Improved;
    }
}