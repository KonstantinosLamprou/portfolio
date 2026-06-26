using FluentValidation;

namespace Backend.Application.UseCases.User;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Der Name darf nicht leer sein.");
        
        RuleFor(x => x.ProfilePictureUrl)
            .NotEmpty().WithMessage("Die Profilbild-URL darf nicht leer sein.");

        RuleFor(x => x.Provider)
            .NotEmpty().WithMessage("Der Auth-Provider darf nicht leer sein.");

        RuleFor(x => x.ProviderSubjectId)
            .NotEmpty().WithMessage("Die Provider-ID darf nicht leer sein.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Die E-Mail-Adresse konnte vom Provider nicht abgerufen werden.");
            
    }
}