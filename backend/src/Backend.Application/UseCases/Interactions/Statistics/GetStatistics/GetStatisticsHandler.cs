using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class GetStatisticsHandler
{
    private readonly IStatisticsInterface _repository;

    public GetStatisticsHandler(IStatisticsInterface repository)
    {
        _repository = repository;
    }

    public async Task<StatisticsResponse?> Handle()
    {
        var statistics = await _repository.GetStatisticsAsync();
        if (statistics == null)
        {
            return null; 
        }
        return new StatisticsResponse(statistics.TotalViews, statistics.TotalLikes);
    }
}
