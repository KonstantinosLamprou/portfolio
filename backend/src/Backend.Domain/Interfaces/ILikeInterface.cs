using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Interfaces
{
    public interface ILikeInterface
    {
        Task<Like?> GetLikeAsync(int contentId, Guid userId);

        // Speichert neue oder aktualisierte Likes in der DB
        Task SaveChangesAsync();

        void AddLike(Like like);
    }
}
