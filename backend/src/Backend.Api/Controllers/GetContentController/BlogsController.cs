using Backend.Application.UseCases.GetContent;
using Backend.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/blogs")] // /api/blogs
public class BlogsController : ControllerBase
{
    private readonly GetAllBlogsHandler _getAllBlogsHandler;
    private readonly GetBlogDetailsHandler _blogDetailshandler; 


    // Handler Dependency Injection 
    public BlogsController(GetAllBlogsHandler getAllBlogsHandler, GetBlogDetailsHandler blogDetailshandler)
    {
        _getAllBlogsHandler = getAllBlogsHandler;
        _blogDetailshandler = blogDetailshandler; 
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBlogs()
    {
        var result = await _getAllBlogsHandler.Handle();

        return Ok(result); 
    }

    [HttpGet("{id}")] // /api/blogs/123
    [ProducesResponseType(typeof(ContentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogDetails(int id)
    {
        // 1. Versuchen, den aktuellen User auszulesen (falls er einen Token hat)
        Guid? userId = null;
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userIdString))
        {
            userId = Guid.Parse(userIdString);
        }

        // 2. Handler aufrufen
        var result = await _blogDetailshandler.Handle(id, userId);

        // 3. Wenn null zurückkommt, existiert der Blog nicht
        if (result == null)
        {
            return NotFound(new { message = "Blog nicht gefunden." });
        }

        return Ok(result);
    }

}