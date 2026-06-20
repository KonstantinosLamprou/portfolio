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
            throw new KeyNotFoundException($"Content mit ID {contentId} nicht gefunden.");


        var allComments = await _commentRepository.GetCommentsByContentIdAsync(content.Id);

        var flatDtos = allComments.Select(c => new CommentResponseDto(
            Id: c.Id,
            Text: c.IsDeleted ? "[Kommentar gelöscht]" : c.Text, 
            CreatedAt: c.CreatedAt,
            Author: c.IsDeleted ? new AuthorDto(
                Id: Guid.Empty,
                Name: "[Gelöschter Benutzer]",
                ProfilePictureUrl: null,
                Role: UserRole.Standard 
            ) :
                new AuthorDto(
                Id: c.Author.Id,
                Name: c.Author.Name,
                ProfilePictureUrl: c.Author.ProfilePictureUrl,
                Role: c.Author.Role
            ),
            CurrentUserVote: currentUserId.HasValue ? c.Votes.FirstOrDefault(v => v.UserId == currentUserId.Value)?.IsUpvote : null,
            IsDeleted: c.IsDeleted,
            Upvotes: c.IsDeleted ? 0 : c.Upvotes,
            Downvotes: c.IsDeleted ? 0 : c.Downvotes,
            ParentCommentId: c.ParentCommentId,
            Replies: new List<CommentResponseDto>()
        )).ToList();

        var topLevelComments = new List<CommentResponseDto>();

        var commentLookup = flatDtos.ToDictionary(c => c.Id);

        foreach (var comment in flatDtos)
        {
            if (comment.ParentCommentId == null)
            {
                // Es ist ein Hauptkommentar -> kommt in die oberste Liste
                topLevelComments.Add(comment);
            }
            else
            {
                // Wenn es ein Reply ist -> suchen wir den Parent und fügen ihn dort in die Liste ein
                if (commentLookup.TryGetValue(comment.ParentCommentId.Value, out var parent))
                {
                    parent.Replies!.Add(comment);
                }
            }
        }   

        return topLevelComments;
    }

}
