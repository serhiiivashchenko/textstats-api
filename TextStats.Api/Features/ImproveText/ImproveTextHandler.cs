using MediatR;

public class ImproveTextHandler
    : IRequestHandler<ImproveTextCommand, string>
{
    private readonly ITextImprover _textImprover;
    public ImproveTextHandler(ITextImprover textImprover)
    {
        _textImprover = textImprover;
    }

    public async Task<string> Handle(ImproveTextCommand request, CancellationToken cancellationToken)
    {
        return await _textImprover.ImproveTextAsync(request.Text, cancellationToken);
    }
}