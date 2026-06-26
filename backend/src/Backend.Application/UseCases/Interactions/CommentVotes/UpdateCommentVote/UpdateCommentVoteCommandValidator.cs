using FluentValidation;

namespace Backend.Application.UseCases.Interactions;

public class UpdateCommentVoteCommandValidator : AbstractValidator<UpdateCommentVoteCommand>
{
    public UpdateCommentVoteCommandValidator()
    {
        
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("Die Kommentar-ID darf nicht leer sein.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("Die User-ID darf nicht leer sein.");
    }
}