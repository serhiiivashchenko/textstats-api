using FluentValidation;

public class ImproveTextValidator : AbstractValidator<ImproveTextCommand>
{
    public ImproveTextValidator()
    {
        RuleFor(x => x.Text)
        .NotEmpty()
        .MaximumLength(2000);
    }
}