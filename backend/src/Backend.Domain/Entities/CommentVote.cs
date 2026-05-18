using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Entities
{
    public class CommentVote
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
        // true = Daumen hoch (Upvote), false = Daumen runter (Downvote)
        public bool IsUpvote { get; set; }
    }
}
