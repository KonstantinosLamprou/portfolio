using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.Content
{
    public class GetAllBlogsHandler : IQueryHandler<GetAllBlogsQuery, IEnumerable<ContentListResponse>>
    {
        private readonly IBlogInterface _repository; 

        public GetAllBlogsHandler(IBlogInterface repository)
        {
            _repository = repository; 
        }

        public async Task<IEnumerable<ContentListResponse>> HandleAsync(GetAllBlogsQuery query, CancellationToken cancellationToken = default)
        {
            var blogs = await _repository.GetAllBlogsAsync(cancellationToken);

            //Mapping Entity -> DTO
            return blogs.Select(b => new ContentListResponse(
                Id: b.Id,
                Title: b.Title,
                Slug: b.Slug,
                DateOfCreation: b.DateOfCreation,
                ImgSrc: b.ImgSrc,
                Description: b.Description,
                Views: b.Views,
                LikesCount: b.Likes?.Sum(l => l.Count) ?? 0,
                CommentsCount: b.Comments?.Count ?? 0,
                Tags: b.Tags
            ));
        }
    }
}
