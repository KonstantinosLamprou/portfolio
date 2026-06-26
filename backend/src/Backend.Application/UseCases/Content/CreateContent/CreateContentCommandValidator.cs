using FluentValidation;

namespace Backend.Application.UseCases.Content;

public class CreateContentCommandValidator : AbstractValidator<CreateContentCommand>
{
    public CreateContentCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Der Titel darf nicht leer sein.")
            .MaximumLength(100).WithMessage("Der Titel darf maximal 100 Zeichen lang sein.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Der Slug darf nicht leer sein.")
            .Matches(@"^[a-z0-9-]+$").WithMessage("Der Slug darf nur Kleinbuchstaben, Zahlen und Bindestriche enthalten.");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Der Inhaltstyp darf nicht leer sein.")
            .Must(x => new[] { "blog", "project" }.Contains(x.ToLower()))
            .WithMessage("Der Inhaltstyp muss entweder 'blog' oder 'project' sein.");

        RuleFor(x => x.Tags)
            .NotEmpty().WithMessage("Mindestens ein Tag ist erforderlich.")
            .Must(x => x.Length <= 5).WithMessage("Es dürfen maximal 5 Tags vergeben werden.");
    }
}