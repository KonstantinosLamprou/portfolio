using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.GetContent; 

public class GetProjectDetailsHandler
{
    private readonly IProjectInterface _repository;

    public GetProjectDetailsHandler(IProjectInterface repository)
    {
        _repository = repository;
    }

    public async Task<ContentDetailResponse?> Handle(string slug, Guid? currentUserId)
    {
        var projectDetails = await _repository.GetProjectDetailsBySlugAsync(slug);
        
        if (projectDetails == null)
        {
            return null;
        }

        return new ContentDetailResponse(
            Id: projectDetails.Id,
            Title: projectDetails.Title,
            Slug: projectDetails.Slug,
            DateOfCreation: projectDetails.DateOfCreation,
            ImgSrc: projectDetails.ImgSrc,
            Description: projectDetails.Description,

            // Da Projekte aktuell keine verschachtelten Content-Blöcke haben, geben wir hier eine leere Liste zurück.
            Content: projectDetails.Content?.Select(c => new ContentBlockDto(
                c.Id, 
                c.Type, 
                JsonDocument.Parse(c.Data).RootElement                
            )).ToList() ?? new List<ContentBlockDto>(),

            Views: projectDetails.Views,
            LikesCount: projectDetails.Likes?.Sum(l => l.Count) ?? 0,
            CommentsCount: projectDetails.Comments?.Count ?? 0,
            Tags: projectDetails.Tags,

            // Prüft, ob der aktuelle User in der Like-Liste steht. Wenn ja, gib den Count, sonst 0.
            CurrentUserLikeCount: currentUserId.HasValue
                ? projectDetails.Likes?.SingleOrDefault(l => l.UserId == currentUserId.Value)?.Count ?? 0
                : 0,

            Author: new AuthorDto(projectDetails.Author.Id, projectDetails.Author.Name, projectDetails.Author.ProfilePictureUrl, projectDetails.Author.Role)
        );
        
    }
    
}
