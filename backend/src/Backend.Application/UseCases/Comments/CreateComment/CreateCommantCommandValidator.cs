using Backend.Application.UseCases.Comments;
using FluentValidation;

namespace Backend.Application.UseCases.Comments; 

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.request.Text)
            .NotEmpty().WithMessage("Die Nachricht darf nicht leer sein.")
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Die Nachricht darf nicht nur aus Leerzeichen bestehen.")
            .Matches(@"^[^<>]*$").WithMessage("HTML-Tags oder Skripte sind nicht erlaubt.")
            .MaximumLength(500).WithMessage("Die Nachricht darf maximal 500 Zeichen lang sein.");
    
        RuleFor(x => x.authorId)
            .NotEmpty().WithMessage("Die AuthorId darf nicht leer sein.");
    }
}