using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Contracts;

public record VoteResponse(
    Guid User,
    Guid CommentId,
    bool IsUpvote
    );


public record CreateVote(
     bool IsUpvote,
     Guid CommentId
    ); 
