using System.Text.Json;
using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Content;

public class CreateContentHandler : ICommandHandler<CreateContentCommand, ContentDetailResponse>
{
    private readonly IBlogInterface _blogRepository;
    private readonly IProjectInterface _projectRepository;

    public CreateContentHandler(IBlogInterface blogRepository, IProjectInterface projectRepository)
    {
        _blogRepository = blogRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ContentDetailResponse> HandleAsync(CreateContentCommand command, CancellationToken cancellationToken = default)
    {
        ContentBase content = command.ContentType.ToLower() switch
        {
            "blog" => new Blog
            {
                Title = command.Title,
                Slug = command.Slug,
                ImgSrc = command.ImgSrc,
                Description = command.Description,
                Content = command.Content
                    .Select(c => new ContentBlock 
                    { 
                        Id = c.Id,
                        Type = c.Type,
                        Data = c.Data.GetRawText() 
                    })
                    .ToList(),
                AuthorId = command.CurrentUserId, 
                Tags = command.Tags
            },
            "project" => new Project
            {
                Title = command.Title,
                Slug = command.Slug,
                ImgSrc = command.ImgSrc,
                Description = command.Description,
                Content = command.Content
                    .Select(c => new ContentBlock 
                    { 
                        Id = c.Id,
                        Type = c.Type,
                        Data = c.Data.GetRawText() 
                    })
                    .ToList(),
                AuthorId = command.CurrentUserId,
                Tags = command.Tags
            },
            _ => throw new InvalidOperationException($"Ungültiger ContentType: {command.ContentType}")
        };

        ContentBase savedContent = content switch
        {
            Blog blog => await _blogRepository.SaveBlogAsync(blog, cancellationToken),
            Project project => await _projectRepository.SaveProjectAsync(project, cancellationToken),
            _ => throw new InvalidOperationException()
        };

        // Mapping DTO 
        var author = savedContent.Author;
        return new ContentDetailResponse(
            Id: savedContent.Id,
            Title: savedContent.Title,
            Slug: savedContent.Slug,
            DateOfCreation: savedContent.DateOfCreation,
            ImgSrc: savedContent.ImgSrc,
            Description: savedContent.Description,
            Content: savedContent.Content
                .Select(c => new ContentBlockDto(
                    c.Id, 
                    c.Type, 
                    JsonDocument.Parse(c.Data).RootElement                
                ))
                .ToList(),
            Views: savedContent.Views,
            LikesCount: savedContent.Likes?.Sum(l => l.Count) ?? 0,
            CommentsCount: savedContent.Comments?.Count ?? 0,
            Tags: savedContent.Tags,
            Author: new AuthorDto(author.Id, author.Name, author.ProfilePictureUrl, author.Role),
            CurrentUserLikeCount: 0
        );
    }
}