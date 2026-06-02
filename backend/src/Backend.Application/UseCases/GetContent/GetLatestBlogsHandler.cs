using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Backend.Application.UseCases.GetContent
{
    public class GetLatestBlogsHandler
    {
        private readonly IBlogInterface _repository;

        public GetLatestBlogsHandler(IBlogInterface repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ContentListResponse>> Handle(int count = 3)
        {
            var blogs = await _repository.GetLatestBlogsAsync(count); 

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
                CommentsCount: b.Comments?.Count ?? 0
            ));
        }
    }
}
