using System.Text.Json;
using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Comments;

public class GetCommentsHandler
{
    private readonly IBlogInterface _blogRepository;
    private readonly IProjectInterface _projectRepository;
    private readonly ICommentInterface _commentRepository;

    public GetCommentsHandler(IBlogInterface blogRepository, IProjectInterface projectRepository, ICommentInterface commentRepository)
    {
        _blogRepository = blogRepository;
        _projectRepository = projectRepository;
        _commentRepository = commentRepository;
    }

    public async Task<List<CommentResponseDto>> Handle(string contentType, int contentId, Guid? currentUserId)
    {
        ContentBase? content = contentType.ToLower() switch 
        {
            "blog" => await _blogRepository.GetBlogByIdAsync(contentId),
            "project" => await _projectRepository.GetProjectByIdAsync(contentId),
            _ => throw new ArgumentException($"Ungültiger ContentType: {contentType}")
        };

        if (content == null)
            throw new KeyNotFoundException($"Content mit ID {contentType} nicht gefunden.");


        var comments = await _commentRepository.GetCommentsByContentIdAsync(content.Id);

        return comments.Select(c => new CommentResponseDto(
            Id: c.Id,
            Text: c.Text,
            CreatedAt: c.CreatedAt,
            Author: new AuthorDto(
                Id: c.Author.Id,
                Name: c.Author.Name,
                ProfilePictureUrl: c.Author.ProfilePictureUrl,
                Role: c.Author.Role
            ),
            Upvotes: c.Upvotes,
            Downvotes: c.Downvotes,
            ParentCommentId: c.ParentCommentId,
            Replies: new List<CommentResponseDto>()
        )).ToList();
    }

}
