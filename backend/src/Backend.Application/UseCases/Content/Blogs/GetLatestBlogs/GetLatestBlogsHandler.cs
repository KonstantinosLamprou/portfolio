using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Content
{
    public class GetLatestBlogsHandler : IQueryHandler<GetLatestBlogsQuery, IEnumerable<ContentListResponse>>
    {
        private readonly IBlogInterface _repository;

        public GetLatestBlogsHandler(IBlogInterface repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ContentListResponse>> HandleAsync(GetLatestBlogsQuery query, CancellationToken cancellationToken = default)
        {
            var blogs = await _repository.GetLatestBlogsAsync(query.Count, cancellationToken); 

            //Mapping Entity -> DTO
            return blogs.Select(b => new ContentListResponse(
                Id: b.Id,
                Title: b.Title,
                Slug: b.Slug,
                DateOfCreation: b.DateOfCreation,
                ImgSrc: b.ImgSrc,
                Description: b.Description,
                Views: b.Views,
                Tags: b.Tags,
                LikesCount: b.Likes?.Sum(l => l.Count) ?? 0,
                CommentsCount: b.Comments?.Count ?? 0
            ));
        }
    }
}
