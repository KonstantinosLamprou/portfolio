using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Domain.Contracts;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Comments;

public class GetCommentByIdHandler : IQueryHandler<GetCommentByIdQuery, CommentResponseDto?>
{
    private readonly ICommentInterface _repository;

    public GetCommentByIdHandler(ICommentInterface repository)
    {
        _repository = repository;
    }

    public async Task<CommentResponseDto?> HandleAsync(GetCommentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var comment = await _repository.GetCommentByIdAsync(query.CommentId);

        if (comment == null)
            return null;

        return new CommentResponseDto
        (
            Id: comment.Id,
            ContentId: comment.ContentId,
            Text: comment.IsDeleted ? "[Kommentar gelöscht]" : comment.Text,
            CreatedAt: comment.CreatedAt,
            Author: comment.IsDeleted ? 
                new AuthorDto(
                    Id: Guid.Empty,
                    Name: "[Gelöschter Benutzer]",
                    ProfilePictureUrl: null,
                    Role: UserRole.Standard
            ) :
                new AuthorDto(
                    Id: comment.Author.Id,
                    Name: comment.Author.Name,
                    ProfilePictureUrl: comment.Author.ProfilePictureUrl,
                    Role: comment.Author.Role
                ),
            CurrentUserVote: query.CurrentUserId.HasValue ? comment.Votes.FirstOrDefault(v => v.UserId == query.CurrentUserId.Value)?.IsUpvote : null,
            IsDeleted: comment.IsDeleted,
            Upvotes: comment.IsDeleted ? 0 : comment.Upvotes,
            Downvotes: comment.IsDeleted ? 0 : comment.Downvotes,
            ParentCommentId: comment.ParentCommentId,
            Replies: new List<CommentResponseDto>()
        );
    }
}