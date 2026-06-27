using System.Text.Json;
using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Comments;

public class CreateCommentHandler : ICommandHandler<CreateCommentCommand, CommentResponseDto>
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

    public async Task<CommentResponseDto> HandleAsync(CreateCommentCommand command, CancellationToken cancellationToken = default)
    {
        // Hier wird die AuthorId aus dem Request benötigt. In der Regel sollte diese aus dem Authentifizierungs-Token oder Cookie stammen.
        Guid AuthorId = command.authorId; // Dies sollte aus dem Request kommen, nicht hartcodiert sein.

        // Entscheidung: Blog oder Project?
        ContentBase? content = command.request.ContentType.ToLower() switch
        {
            "blog" => await _blogRepository.GetBlogByIdAsync(command.request.ContentId),
            "project" => await _projectRepository.GetProjectByIdAsync(command.request.ContentId),
            _ => throw new ArgumentException($"Ungültiger ContentType: {command.request.ContentType}")
        };

        if (content == null)
            throw new KeyNotFoundException($"Content mit ID {command.request.ContentId} nicht gefunden.");

        // Kommentar erstellen
        Comment comment = new Comment
        {
            ContentId = content.Id,
            AuthorId = AuthorId, 
            Text = command.request.Text,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = command.request.ParentCommentId == Guid.Empty ? null : command.request.ParentCommentId
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
            ContentId: comment.ContentId,
            Text: comment.Text,
            CreatedAt: comment.CreatedAt,
            Author: authorDto,
            Upvotes: 0,    
            Downvotes: 0,
            CurrentUserVote: null,
            ParentCommentId: comment.ParentCommentId,
            IsDeleted: false,
            Replies: new List<CommentResponseDto>()
        );
    }
}