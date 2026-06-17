using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Domain.Contracts;


public record CreateVoteDto(
     bool IsUpvote,
     Guid CommentId
);

public record UpdateVoteDto(
     bool IsUpvote,
     Guid CommentId
);
