using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TextDocumentConfiguration : IEntityTypeConfiguration<TextDocument>
{

    public void Configure(EntityTypeBuilder<TextDocument> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OriginalText)
            .HasConversion(
                sourceText => sourceText.Value,
                value => new SourceText(value));
        builder.Property(x => x.Status)
            .HasConversion<string>();
    }
}