using FluentValidation;

namespace Backend.Application.UseCases.Interactions;

public class CreateCommentVoteCommandValidator : AbstractValidator<CreateCommentVoteCommand>
{
    public CreateCommentVoteCommandValidator()
    {
        
        RuleFor(x => x.CreateVoteDto.CommentId)
            .NotEmpty().WithMessage("Die Kommentar-ID darf nicht leer sein.");

        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("Die User-ID darf nicht leer sein.");
    }
}