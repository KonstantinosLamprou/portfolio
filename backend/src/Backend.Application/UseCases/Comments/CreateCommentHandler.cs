using System.Text.Json;
using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Comments;

public class CreateCommentHandler
{
    private readonly IBlogInterface _blogRepository;
    private readonly IProjectInterface _projectRepository;
    private readonly ICommentInterface _commentRepository;

    public CreateCommentHandler(IBlogInterface blogRepository, IProjectInterface projectRepository, ICommentInterface commentRepository)
    {
        _blogRepository = blogRepository;
        _projectRepository = projectRepository;
        _commentRepository = commentRepository;
    }

    public async Task<CommentResponseDto> Handle(CreateCommentRequest request, Guid authorId)
    {
        // Entscheidung: Blog oder Project?
        ContentBase? content = request.ContentType.ToLower() switch
        {
            "blog" => await _blogRepository.GetBlogByIdAsync(request.ContentId),
            "project" => await _projectRepository.GetProjectByIdAsync(request.ContentId),
            _ => throw new ArgumentException($"Ungültiger ContentType: {request.ContentType}")
        };

        if (content == null)
            throw new KeyNotFoundException($"Content mit ID {request.ContentId} nicht gefunden.");

        // Kommentar erstellen
        Comment comment = new Comment
        {
            ContentId = content.Id,
            AuthorId = authorId, // Wird diese aus dem Cookie im Frontend übergeben? Oder brauchen wir hier die AuthorDto aus dem Request?
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = request.ParentCommentId == Guid.Empty ? null : request.ParentCommentId
        };

        // Kommentar speichern
        await _commentRepository.AddCommentAsync(comment);

        var authorDto = new AuthorDto(
            Id: comment.Author.Id,
            Name: comment.Author.Name,
            ProfilePictureUrl: comment.Author.ProfilePictureUrl,
            Role: comment.Author.Role // Enum-Mapping
        );

        return new CommentResponseDto(
            Id: comment.Id,
            Text: comment.Text,
            CreatedAt: comment.CreatedAt,
            Author: authorDto,
            Upvotes: 0,    
            Downvotes: 0,
            ParentCommentId: comment.ParentCommentId,
            IsDeleted: false,
            Replies: new List<CommentResponseDto>()
        );

    }
}