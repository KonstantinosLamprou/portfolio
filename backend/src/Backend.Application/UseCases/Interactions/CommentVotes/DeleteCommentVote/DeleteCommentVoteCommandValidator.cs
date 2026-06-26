using FluentValidation;

namespace Backend.Application.UseCases.Interactions;

public class DeleteCommentVoteCommandValidator : AbstractValidator<DeleteCommentVoteCommand>
{
    public DeleteCommentVoteCommandValidator()
    {
        
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("Die Kommentar-ID darf nicht leer sein.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("Die User-ID darf nicht leer sein.");
    }
}