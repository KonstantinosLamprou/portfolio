using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class AddLikeHandler
{
    private readonly ILikeInterface _repository;

    public AddLikeHandler(ILikeInterface repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(int contentId, Guid userId)
    {
        // Gibt es den Like schon? 
        var existingLike = await _repository.GetLikeAsync(contentId, userId);

        // Noch nie geliked
        if (existingLike == null)
        {
            existingLike = new Like
            {
                ContentId = contentId,
                UserId = userId,
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
en.
        }

        // speichern in der Datenbank 
        await _repository.SaveChangesAsync();

        return existingLike.Count;
    }
}