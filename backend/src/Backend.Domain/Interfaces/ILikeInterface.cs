using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Interfaces
{
    public interface ILikeInterface
    {
        Task<Like?> GetLikeAsync(int contentId, Guid userId, CancellationToken cancellationToken = default);

        // Speichert neue oder aktualisierte Likes in der DB
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        void AddLike(Like like);
    }
}
