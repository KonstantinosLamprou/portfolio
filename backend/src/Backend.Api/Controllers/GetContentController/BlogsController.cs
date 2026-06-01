using Backend.Application.UseCases.GetContent;
using Backend.Application.UseCases.SaveContent;
using Backend.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/blogs")] // /api/blogs
public class BlogsController : ControllerBase
{
    private readonly GetAllBlogsHandler _getAllBlogsHandler;
    private readonly GetBlogDetailsHandler _blogDetailshandler; 
    private readonly CreateContentHandler _createContentHandler;


    // Handler Dependency Injection 
    public BlogsController(GetAllBlogsHandler getAllBlogsHandler, GetBlogDetailsHandler blogDetailshandler, CreateContentHandler createContentHandler)
    {
        _getAllBlogsHandler = getAllBlogsHandler;
        _blogDetailshandler = blogDetailshandler; 
        _createContentHandler = createContentHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBlogs()
    {
        var result = await _getAllBlogsHandler.Handle();

        return Ok(result); 
    }

    [HttpGet("{slug}")] // /api/blogs/123
    [ProducesResponseType(typeof(ContentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogDetails(string slug)
    {
        // 1. Versuchen, den aktuellen User auszulesen (falls er einen Token hat)
        Guid? userId = GetCurrentUserId(); 

        // 2. Handler aufrufen
        var result = await _blogDetailshandler.Handle(slug, userId);

        // 3. Wenn null zurückkommt, existiert der Blog nicht
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
        return CreatedAtAction(nameof(GetBlogDetails), new { slug = result.Slug }, result);
    }

    // ← Helper-Methode
    private Guid? GetCurrentUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            return null;

        return Guid.TryParse(userIdString, out var userId) ? userId : null;
    }

}