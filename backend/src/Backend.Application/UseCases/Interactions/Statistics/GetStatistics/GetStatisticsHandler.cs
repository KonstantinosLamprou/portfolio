using Backend.Domain.Contracts;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class GetStatisticsHandler : IQueryHandler<GetStatisticsQuery, StatisticsResponse?>
{
    private readonly IStatisticsInterface _repository;

    public GetStatisticsHandler(IStatisticsInterface repository)
    {
        _repository = repository;
    }

    public async Task<StatisticsResponse?> HandleAsync(GetStatisticsQuery query, CancellationToken cancellationToken = default)
    {
        var statistics = await _repository.GetStatisticsAsync(cancellationToken);
        if (statistics == null)
        {
            return null; 
        }
        return new StatisticsResponse(statistics.TotalViews, statistics.TotalLikes);
    }
}
