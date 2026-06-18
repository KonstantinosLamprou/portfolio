using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace Backend.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Zu welchem Blog/Projekt gehört der Kommentar?
        public int ContentId { get; set; }
        public ContentBase Content { get; set; }

        // Nullable Guid, denn Hauptkommentare haben keinen Parent
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }

        // Welcher User hat den Kommentar geschrieben?
        public Guid AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public ICollection<CommentVote> Votes { get; set; } = new List<CommentVote>();

        public int Upvotes => Votes.Count(v => v.IsUpvote);
        public int Downvotes => Votes.Count(v => !v.IsUpvote);

        public bool IsDeleted { get; set; } = false;
    }
}
