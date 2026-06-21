using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class UpdateStatisticsHandler
{
    private readonly IStatisticsInterface _repository;

    public UpdateStatisticsHandler(IStatisticsInterface repository)
    {
        _repository = repository;
    }

    public async Task Handle(int? viewToAdd, int? likeToAdd)
    {
        var statistics = await _repository.GetStatisticsAsync() ?? throw new InvalidOperationException("Statistics not found.");

        if (viewToAdd.HasValue)
        {
            statistics.TotalViews += viewToAdd.Value;
        }
        if (likeToAdd.HasValue)
        {
            statistics.TotalLikes += likeToAdd.Value;
        }

        await _repository.UpdateStatisticsAsync(statistics);
    }
}