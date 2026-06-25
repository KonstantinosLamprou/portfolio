using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Content;

public class GetBlogDetailsHandler : IQueryHandler<GetBlogsDetailsQuery, ContentDetailResponse?>
{
    private readonly IBlogInterface _repository;

    public GetBlogDetailsHandler(IBlogInterface repository)
    {
        _repository = repository;
    }

    public async Task<ContentDetailResponse?> HandleAsync(GetBlogsDetailsQuery query, CancellationToken cancellationToken = default)
    {
        var blogDetails = await _repository.GetBlogWithDetailsBySlugAsync(query.Slug, cancellationToken);

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
            Tags: blogDetails.Tags,

            // Prüft, ob der aktuelle User in der Like-Liste steht. Wenn ja, gib den Count, sonst 0.
            CurrentUserLikeCount: query.CurrentUserId.HasValue
                ? blogDetails.Likes?.SingleOrDefault(l => l.UserId == query.CurrentUserId.Value)?.Count ?? 0
                : 0,

            Author: new AuthorDto(blogDetails.Author.Id, blogDetails.Author.Name, blogDetails.Author.ProfilePictureUrl, blogDetails.Author.Role)
        );

    }

}
