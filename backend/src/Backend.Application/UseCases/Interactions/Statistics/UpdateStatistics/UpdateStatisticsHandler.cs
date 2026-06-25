using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models; 

namespace Backend.Application.UseCases.Interactions;

public class UpdateStatisticsHandler : ICommandHandler<UpdateStatisticsCommand, Unit>
{
    private readonly IStatisticsInterface _repository;

    public UpdateStatisticsHandler(IStatisticsInterface repository)
    {
        _repository = repository;
    }

    public async Task<Unit> HandleAsync(UpdateStatisticsCommand command, CancellationToken cancellationToken = default)
    {
        var statistics = await _repository.GetStatisticsAsync(cancellationToken) ?? throw new InvalidOperationException("Statistics not found.");

        if (command.ViewsToAdd.HasValue)
        {
            statistics.TotalViews += command.ViewsToAdd.Value;
        }
        if (command.LikesToAdd.HasValue)
        {
            statistics.TotalLikes += command.LikesToAdd.Value;
        }

        await _repository.UpdateStatisticsAsync(statistics, cancellationToken);

        return Unit.Value;
    }
}