using System;
using System.Collections.Generic;
using System.Text;



namespace Backend.Domain.Entities
{
    public class ApplicationUser 
    {
        public Guid Id { get; set; }
        public required string AuthProvider { get; set; }
        public required string ProviderSubjectId { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public UserRole Role { get; set; } = UserRole.Standard;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLoginAt { get; set; }

        public ICollection<Blog> CreatedBlogs { get; set; } = new List<Blog>();
        public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

        // Ein User kann mehrere Kommentare schreiben
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Ein User kann mehrere Gästebucheinträge hinterlassen
        public ICollection<GuestbookEntry> GuestbookEntries { get; set; } = new List<GuestbookEntry>();
    }

    public enum UserRole
    {
        Standard, 
        Admin
    }
}
