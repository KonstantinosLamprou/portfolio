using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities
{
    public class Like
    {
        public DateTime LastLikedAt { get; set; } = DateTime.UtcNow;
        public int Count { get; set; } = 1;

        // Welcher Blog/Projekt wurde geliked?
        public int ContentId { get; set; }
        public ContentBase Content { get; set; }

        // Welcher User hat den Like vergeben?
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
