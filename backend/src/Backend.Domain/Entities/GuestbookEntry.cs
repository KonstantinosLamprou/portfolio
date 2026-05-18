using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities
{
    public class GuestbookEntry
    {
        public Guid Id { get; set; }

        public required string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Beziehung zum User ---
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
