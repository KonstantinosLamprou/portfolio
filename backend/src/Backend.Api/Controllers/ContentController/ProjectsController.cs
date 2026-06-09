using Backend.Domain.Contracts;
using Backend.Application.UseCases.GetContent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend.Application.UseCases.SaveContent;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/projects")] // /api/projects
public class ProjectsController : ControllerBase
{
    private readonly GetAllProjectsHandler _getAllProjectsHandler; 
    private readonly GetProjectDetailsHandler _projectDetailshandler;
    private readonly CreateContentHandler _createContentHandler;

    public ProjectsController(GetAllProjectsHandler getAllProjectsHandler, GetProjectDetailsHandler projectDetailshandler, CreateContentHandler createContentHandler)
    {
        _getAllProjectsHandler = getAllProjectsHandler; 
        _projectDetailshandler = projectDetailshandler;
        _createContentHandler = createContentHandler;
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
        Guid? userId = GetCurrentUserId(); 

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
        var userId = GetCurrentUserId();

        if (!userId.HasValue)
            return Unauthorized(new { message = "User nicht authentifiziert." });

        var result = await _createContentHandler.Handle(request, userId.Value);
        return CreatedAtAction(nameof(GetProjectDetails), new { slug = result.Slug }, result);
    }

    // TODO: Auslagern
    // Helper-Methode
    private Guid? GetCurrentUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            return null;

        return Guid.TryParse(userIdString, out var userId) ? userId : null;
    }



}
