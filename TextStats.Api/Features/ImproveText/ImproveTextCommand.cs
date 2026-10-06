using MediatR;

public record ImproveTextCommand(string Text) : IRequest<string>;