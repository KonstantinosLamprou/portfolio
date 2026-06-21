using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Application.UseCases.GetContent
{
    public class GetAllProjectsHandler
    {
        private readonly IProjectInterface _repository; 

        public GetAllProjectsHandler(IProjectInterface repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContentListResponse>> Handle()
        {
            var projects = await _repository.GetAllProjectsAsync();

            //Mapping Entity -> DTO
            return projects.Select(p => new ContentListResponse(
                Id: p.Id,
                Title: p.Title,
                Slug: p.Slug,
                DateOfCreation: p.DateOfCreation,
                ImgSrc: p.ImgSrc,
                Description: p.Description,
                Views: p.Views,
                Tags: p.Tags,
                LikesCount: p.Likes?.Sum(l => l.Count) ?? 0,
                CommentsCount: p.Comments?.Count ?? 0
            ));
        }
    }
}
