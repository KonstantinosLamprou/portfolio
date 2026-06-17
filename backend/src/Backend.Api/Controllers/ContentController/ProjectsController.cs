using Backend.Domain.Contracts;
using Backend.Application.UseCases.GetContent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend.Application.UseCases.SaveContent;
using Backend.Application.UseCases.Interactions;
using Backend.Api.Helpers;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/projects")] // /api/projects
public class ProjectsController : ControllerBase
{
    private readonly GetAllProjectsHandler _getAllProjectsHandler; 
    private readonly GetProjectDetailsHandler _projectDetailshandler;
    private readonly CreateContentHandler _createContentHandler;
    private readonly UpdateViewsProjectHandler _updateViewsProjectHandler;

    public ProjectsController(
        GetAllProjectsHandler getAllProjectsHandler, 
        GetProjectDetailsHandler projectDetailshandler, 
        CreateContentHandler createContentHandler,
        UpdateViewsProjectHandler updateViewsProjectHandler)
    {
        _getAllProjectsHandler = getAllProjectsHandler; 
        _projectDetailshandler = projectDetailshandler;
        _createContentHandler = createContentHandler;
        _updateViewsProjectHandler = updateViewsProjectHandler;
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProjects()
    {
        var result = await _getAllProjectsHandler.Handle();

        return Ok(result);
    }

    [HttpGet("{slug}")] // /api/blogs/123
    [ProducesResponseType(typeof(ContentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectDetails(string slug)
    {
        // Versuchen, den aktuellen User auszulesen (falls er einen Token hat)
        Guid? userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User); 

        // Handler aufrufen
        var result = await _projectDetailshandler.Handle(slug, userId);

        // Wenn null zurückkommt, existiert der Blog nicht
        if (result == null)
        {
            return NotFound(new { message = "Blog nicht gefunden." });
        }

        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize]  
    public async Task<IActionResult> CreateContent([FromBody] CreateBlogRequest request)
    {
        var userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (!userId.HasValue)
            return Unauthorized(new { message = "User nicht authentifiziert." });

        var result = await _createContentHandler.Handle(request, userId.Value);
        return CreatedAtAction(nameof(GetProjectDetails), new { slug = result.Slug }, result);
    }

    [HttpPatch("{projectId}/view")]
    [ProducesResponseType(typeof(UpdateViewsContentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IncrementProjectViews(int projectId)
    {        
        var result = await _updateViewsProjectHandler.Handle(projectId);
        return Ok(result);  
    }


}
