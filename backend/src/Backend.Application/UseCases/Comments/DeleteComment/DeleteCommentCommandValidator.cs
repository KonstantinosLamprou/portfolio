using FluentValidation;

namespace Backend.Application.UseCases.Comments; 

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("Die Kommentar-ID darf nicht leer sein.");
            
        RuleFor(x => x.CurrentUserId)
            .NotEmpty().WithMessage("Die CurrentUserId darf nicht leer sein.");
    }
}