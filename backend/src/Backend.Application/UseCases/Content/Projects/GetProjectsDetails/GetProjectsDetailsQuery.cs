using Backend.Application.Common.Interfaces;


namespace Backend.Application.UseCases.Content;

public record GetProjectDetailsQuery(string Slug, Guid? CurrentUserId) : ICacheQueryQuery
{
    public string CacheKey => $"ProjectDetails_{Slug}_{CurrentUserId}";
    
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
