using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Application.UseCases.Comments;
using Backend.Domain.Contracts; 

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly CreateCommentHandler _createCommentHandler;

    public CommentsController(CreateCommentHandler createCommentHandler)
    {
        _createCommentHandler = createCommentHandler;
    }
    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
    {
        // ID aus den Token-Claims des angemeldeten Users holen
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!Guid.TryParse(userIdString, out Guid authorId))
            return Unauthorized();

        var responseDto = await _createCommentHandler.Handle(request, authorId);

        return Ok(responseDto);
    }

}
