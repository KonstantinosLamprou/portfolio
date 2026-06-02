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
    // Votes
    int Upvotes,
    int Downvotes,
    // downvoted (false) oder gar nicht gevotet (null)?
    bool? CurrentUserVote,
    Guid? ParentCommentId,
    // Die Replies sind einfach wieder eine Liste von CommentResponseDto
    List<CommentResponseDto> Replies
);


public record CreateCommentRequest(
    string Text,
    int ContentId, 
    Guid? ParentCommentId
    ); 