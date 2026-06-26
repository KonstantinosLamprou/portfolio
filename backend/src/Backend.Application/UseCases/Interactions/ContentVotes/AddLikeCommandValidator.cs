using FluentValidation;

namespace Backend.Application.UseCases.Interactions;

public class AddLikeCommandValidator : AbstractValidator<AddLikeCommand>
{
    public AddLikeCommandValidator()
    {

        RuleFor(x => x.ContentId)
            .NotEmpty().WithMessage("Die Content-ID darf nicht leer sein.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("Die User-ID darf nicht leer sein.");
    }
}