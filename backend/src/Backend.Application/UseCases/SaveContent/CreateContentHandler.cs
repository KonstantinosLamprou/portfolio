using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.SaveContent;

public class CreateContentHandler
{
    private readonly IBlogInterface _blogRepository;
    private readonly IProjectInterface _projectRepository;

    public CreateContentHandler(IBlogInterface blogRepository, IProjectInterface projectRepository)
    {
        _blogRepository = blogRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ContentDetailResponse> Handle(CreateBlogRequest request, Guid authorId)
    {
        // Entscheidung: Blog oder Project?
        ContentBase content = request.ContentType.ToLower() switch
        {
            "blog" => new Blog
            {
                Title = request.Title,
                Slug = request.Slug,
                ImgSrc = request.ImgSrc,
                Description = request.Description,
                Content = request.Content
                    .Select(c => new ContentBlock 
                    { 
                        Id = c.Id,
                        Type = c.Type,
                        Data = c.Data
                    })
                    .ToList(),
                AuthorId = authorId
            },
            "project" => new Project
            {
                Title = request.Title,
                Slug = request.Slug,
                ImgSrc = request.ImgSrc,
                Description = request.Description,
                Content = request.Content
                    .Select(c => new ContentBlock 
                    { 
                        Id = c.Id,
                        Type = c.Type,
                        Data = c.Data
                    })
                    .ToList(),
                AuthorId = authorId
            },
            _ => throw new InvalidOperationException($"Ungültiger ContentType: {request.ContentType}")
        };

        // Je nachdem was es ist, speichern
        ContentBase savedContent = content switch
        {
            Blog blog => await _blogRepository.SaveBlogAsync(blog),
            Project project => await _projectRepository.SaveProjectAsync(project),
            _ => throw new InvalidOperationException()
        };

        // Mapping zu DTO (wie in deinen anderen Handlers)
        var author = savedContent.Author;
        return new ContentDetailResponse(
            Id: savedContent.Id,
            Title: savedContent.Title,
            Slug: savedContent.Slug,
            DateOfCreation: savedContent.DateOfCreation,
            ImgSrc: savedContent.ImgSrc,
            Description: savedContent.Description,
            Content: savedContent.Content
                .Select(c => new ContentBlockDto(c.Id, c.Type, c.Data))
                .ToList(),
            Views: savedContent.Views,
            LikesCount: savedContent.Likes?.Sum(l => l.Count) ?? 0,
            CommentsCount: savedContent.Comments?.Count ?? 0,
            Author: new AuthorDto(author.Id, author.Name, author.ProfilePictureUrl, author.Role),
            CurrentUserLikeCount: 0
        );
    }
}