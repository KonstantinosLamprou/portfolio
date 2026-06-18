using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Contracts;

//Test ob commit klappt ins neue repo 

public record AuthorDto(
    Guid Id,
    string Name,
    string? ProfilePictureUrl,
    UserRole Role // "Admin"-Badge bei eigenen kommentaren 
);

public record CommentResponseDto(
    Guid Id,
    string Text,
    DateTime CreatedAt,
    AuthorDto Author,
    bool IsDeleted, 
    // Votes
    int Upvotes,
    int Downvotes,
    // downvoted (false) oder gar nicht gevotet (null)?
    Guid? ParentCommentId,
    // Die Replies sind einfach wieder eine Liste von CommentResponseDto
    List<CommentResponseDto>? Replies
);

public record UpdateCommentRequest(
    Guid CommentId,
    string Text
);

public record VoteCommentRequest(
    AuthorDto User,
    Guid CommentId,
    bool IsUpvote
);

public record RemoveVoteCommentRequest(
    Guid CommentId
);

public record CreateCommentRequest(
    string Text,
    string ContentType,
    int ContentId, 
    Guid? ParentCommentId
); 

// public record CreateReplyCommentRequest(
//     string Text,
//     string ContentType,
//     int ContentId, 
//     Guid ParentCommentId
// ); 


// public record DeleteCommentRequest(
//     Guid CommentId
// );


// public record ReplyCommentResponseDto(
//     Guid Id,
//     string Text,
//     DateTime CreatedAt,
//     AuthorDto Author,
//     // Votes
//     int Upvotes,
//     int Downvotes,
//     // downvoted (false) oder gar nicht gevotet (null)?
//     bool? CurrentUserVote,
//     Guid ParentCommentId,
//     List<ReplyCommentResponseDto>? Replies
// );