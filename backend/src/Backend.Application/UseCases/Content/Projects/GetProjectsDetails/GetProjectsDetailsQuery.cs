using Backend.Application.Common.Interfaces;


namespace Backend.Application.UseCases.Content;

public record GetProjectDetailsQuery(string Slug, Guid? CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"ProjectDetails_{Slug}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(1);
}
