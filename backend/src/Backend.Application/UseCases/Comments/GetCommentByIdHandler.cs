using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Domain.Contracts;

namespace Backend.Application.UseCases.Comments;

public class GetCommentByIdHandler
{
    private readonly ICommentInterface _repository;

    public GetCommentByIdHandler(ICommentInterface repository)
    {
        _repository = repository;
    }

    public async Task<CommentResponseDto?> Handle(Guid commentId)
    {
        var comment = await _repository.GetCommentByIdAsync(commentId);

        if (comment == null)
            return null;

        return new CommentResponseDto
        (
            Id: comment.Id,
            Text: comment.Text,
            CreatedAt: comment.CreatedAt,
            Author: new AuthorDto(
                Id: comment.Author.Id,
                Name: comment.Author.Name,
                ProfilePictureUrl: comment.Author.ProfilePictureUrl,
                Role: comment.Author.Role
            ),
            Upvotes: comment.Upvotes,
            Downvotes: comment.Downvotes,
            ParentCommentId: comment.ParentCommentId,
            Replies: new List<CommentResponseDto>()
        );
    }
}