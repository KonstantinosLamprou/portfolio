using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;

public class DeleteCommentVoteHandler
{
    private readonly ICommentVoteInterface _repository;

    public DeleteCommentVoteHandler(ICommentVoteInterface repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(Guid commentId, Guid userId)
    {

        var existingVote = await _repository.GetCommentVotesAsync(commentId);
        
        if (!existingVote.Any(c => c.UserId == userId))
        {
            return false; 
        }

        return await _repository.DeleteCommentVoteAsync(commentId, userId);
    }
}