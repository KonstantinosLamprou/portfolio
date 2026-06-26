using FluentValidation;

namespace Backend.Application.UseCases.Comments; 

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        // 1. IDs prüfen
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("Die Kommentar-ID darf nicht leer sein.");
            
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("Die CurrentUserId darf nicht leer sein.");

        // 2. Den neuen Text genauso streng prüfen wie beim Erstellen
        RuleFor(x => x.Request.Text)
            .NotEmpty().WithMessage("Der aktualisierte Text darf nicht leer sein.")
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Der Text darf nicht nur aus Leerzeichen bestehen.")
            .Matches(@"^[^<>]*$").WithMessage("HTML-Tags oder Skripte sind nicht erlaubt.")
            .MaximumLength(500).WithMessage("Der Text darf maximal 500 Zeichen lang sein.");
    }
}