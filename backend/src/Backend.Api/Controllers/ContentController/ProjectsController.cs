using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend.Domain.Contracts;
using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.Content;
using Backend.Api.Helpers;
using Backend.Application.Common.Interfaces;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/projects")] // /api/projects
public class ProjectsController : ControllerBase
{
    private readonly IQueryHandler<GetAllProjectsQuery, IEnumerable<ContentListResponse>> _getAllProjectsHandler;
    private readonly IQueryHandler<GetProjectDetailsQuery, ContentDetailResponse> _projectDetailshandler;
    private readonly ICommandHandler<CreateContentCommand, ContentDetailResponse> _createContentHandler;
    private readonly ICommandHandler<UpdateProjectViewsCommand, UpdateViewsContentResponse> _updateViewsProjectHandler;

    public ProjectsController(
        IQueryHandler<GetAllProjectsQuery, IEnumerable<ContentListResponse>> getAllProjectsHandler, 
        IQueryHandler<GetProjectDetailsQuery, ContentDetailResponse> projectDetailshandler, 
        ICommandHandler<CreateContentCommand, ContentDetailResponse> createContentHandler,
        ICommandHandler<UpdateProjectViewsCommand, UpdateViewsContentResponse> updateViewsProjectHandler)
    {
        _getAllProjectsHandler = getAllProjectsHandler; 
        _projectDetailshandler = projectDetailshandler;
        _createContentHandler = createContentHandler;
        _updateViewsProjectHandler = updateViewsProjectHandler;
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProjects(
        CancellationToken ct = default
    )
    {
        var result = await _getAllProjectsHandler.HandleAsync(new GetAllProjectsQuery(CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty), ct); 

        return Ok(result);
    }

    [HttpGet("{slug}")] // /api/projects/mein-cooles-projekt
    [ProducesResponseType(typeof(ContentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectDetails(
        string slug, 
        CancellationToken cancellationToken
    )
    {
        Guid? userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User); 

        var result = await _projectDetailshandler.HandleAsync(
            new GetProjectDetailsQuery(slug, userId),
            cancellationToken
            );

        if (result == null)
        {
            return NotFound(new { message = "Projekt nicht gefunden." });
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]  
    public async Task<IActionResult> CreateContent([FromBody] CreateContentRequest request, CancellationToken ct)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (!userId.HasValue)
            return Unauthorized(new { message = "User nicht authentifiziert." });

        var result = await _createContentHandler.HandleAsync(new CreateContentCommand(
            ContentType: request.ContentType,
            Title: request.Title,
            Slug: request.Slug,
            ImgSrc: request.ImgSrc,
            Description: request.Description,
            Content: request.Content,
            Tags: request.Tags,
            CurrentUserId: userId.Value
        ), ct);

        return CreatedAtAction(nameof(GetProjectDetails), new { slug = result.Slug }, result);
    }

    [HttpPatch("{projectId}/view")]
    [ProducesResponseType(typeof(UpdateViewsContentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IncrementProjectViews(
        [FromRoute] int projectId, 
        [FromQuery] string slug, 
        CancellationToken cancellationToken)
    {
        
        
        var result = await _updateViewsProjectHandler.HandleAsync(new UpdateProjectViewsCommand(projectId, slug), cancellationToken);

        return Ok(result);  
    }


}
