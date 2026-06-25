using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class AddLikeHandler : ICommandHandler<AddLikeCommand, int>
{
    private readonly ILikeInterface _repository;

    public AddLikeHandler(ILikeInterface repository)
    {
        _repository = repository;
    }

    public async Task<int> HandleAsync(AddLikeCommand command, CancellationToken cancellationToken = default)
    {
        // Gibt es Likes schon? 
        var existingLike = await _repository.GetLikeAsync(command.ContentId, command.CurrentUserId, cancellationToken);

        // Noch nie geliked
        if (existingLike == null)
        {
            existingLike = new Like
            {
                ContentId = command.ContentId,
                UserId = command.CurrentUserId,
                Count = 1
            };
            _repository.AddLike(existingLike);
        }
        // Schon geliked
        else
        {
            if (existingLike.Count >= 3)
            {
                // Hier wirft der Handler die Exception
                throw new InvalidOperationException("Du hast das Limit von 3 Likes für diesen Post erreicht.");
            }

            existingLike.Count++;
            existingLike.LastLikedAt = DateTime.UtcNow;

        }

        // speichern in der Datenbank 
        await _repository.SaveChangesAsync(cancellationToken);

        return existingLike.Count;
    }
}