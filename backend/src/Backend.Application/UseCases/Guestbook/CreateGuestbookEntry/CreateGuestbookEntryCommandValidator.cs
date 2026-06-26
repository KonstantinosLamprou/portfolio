using FluentValidation;

namespace Backend.Application.UseCases.Guestbook; 

public class CreateGuestbookEntryCommandValidator : AbstractValidator<CreateGuestbookEntryCommand>
{
    public CreateGuestbookEntryCommandValidator()
    {
        RuleFor(x => x.Request.Message)
            .NotEmpty().WithMessage("Die Nachricht darf nicht leer sein.")
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Die Nachricht darf nicht nur aus Leerzeichen bestehen.")
            .Matches(@"^[^<>]*$").WithMessage("HTML-Tags oder Skripte sind nicht erlaubt.")
            .MaximumLength(500).WithMessage("Die Nachricht darf maximal 500 Zeichen lang sein.");
    
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Die AuthorId darf nicht leer sein.");
    }
}
