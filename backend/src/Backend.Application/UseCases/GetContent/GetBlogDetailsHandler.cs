using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Backend.Application.UseCases.GetContent;

public class GetBlogDetailsHandler
{
    private readonly IBlogInterface _repository;

    public GetBlogDetailsHandler(IBlogInterface repository)
    {
        _repository = repository;
    }

    public async Task<ContentDetailResponse?> Handle(string slug, Guid? currentUserId)
    {
        var blogDetails = await _repository.GetBlogWithDetailsBySlugAsync(slug);
        
        if (blogDetails == null)
        {
            return null;
        }



        return new ContentDetailResponse(
            Id: blogDetails.Id,
            Title: blogDetails.Title,
            Slug: blogDetails.Slug,
            DateOfCreation: blogDetails.DateOfCreation,
            ImgSrc: blogDetails.ImgSrc,
            Description: blogDetails.Description,

            // Die verschachtelten Content-Blöcke mappen und Sicherheitsnetz: Falls Content null ist, gib eine leere Liste zurück
            Content: blogDetails.Content?.Select(c => new ContentBlockDto(
                c.Id, 
                c.Type, 
                JsonDocument.Parse(c.Data).RootElement                
            )).ToList() ?? new List<ContentBlockDto>(),

            Views: blogDetails.Views,
            LikesCount: blogDetails.Likes?.Sum(l => l.Count) ?? 0,
            CommentsCount: blogDetails.Comments?.Count ?? 0,

            // Prüft, ob der aktuelle User in der Like-Liste steht. Wenn ja, gib den Count, sonst 0.
            CurrentUserLikeCount: currentUserId.HasValue
                ? blogDetails.Likes?.SingleOrDefault(l => l.UserId == currentUserId.Value)?.Count ?? 0
                : 0,

            // HINWEIS zum Author: 
            // In ContentBase hast du aktuell 'public string Author'. Das DTO erwartet ein 'AuthorDto'.
            // Wenn du hier später eine echte Relation zum User aufbaust, machst du:
            Author: new AuthorDto(blogDetails.Author.Id, blogDetails.Author.Name, blogDetails.Author.ProfilePictureUrl, blogDetails.Author.Role)
        );

    }

}
