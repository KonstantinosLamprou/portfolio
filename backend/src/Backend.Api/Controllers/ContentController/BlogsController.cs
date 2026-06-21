using Backend.Application.UseCases.GetContent;
using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.SaveContent;
using Backend.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Api.Helpers; 

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/blogs")] // /api/blogs
public class BlogsController : ControllerBase
{
    private readonly GetAllBlogsHandler _getAllBlogsHandler;
    private readonly GetBlogDetailsHandler _blogDetailshandler; 
    private readonly CreateContentHandler _createContentHandler;
    private readonly GetLatestBlogsHandler _latestBlogsHandler;

    private readonly UpdateViewsBlogHandler _updateViewsBlogHandler;


    // Handler Dependency Injection 
    public BlogsController(
        GetAllBlogsHandler getAllBlogsHandler,
        GetBlogDetailsHandler blogDetailshandler,
        CreateContentHandler createContentHandler,
        GetLatestBlogsHandler latestBlogsHandler, 
        UpdateViewsBlogHandler updateViewsBlogHandler
          )
    {
        _getAllBlogsHandler = getAllBlogsHandler;
        _blogDetailshandler = blogDetailshandler; 
        _createContentHandler = createContentHandler;
        _latestBlogsHandler = latestBlogsHandler;
        _updateViewsBlogHandler = updateViewsBlogHandler;
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
        Guid userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty;

        var result = await _blogDetailshandler.Handle(slug, userId);

        if (result == null)
        {
            return NotFound(new { message = "Blog nicht gefunden." });
        }

        return Ok(result);
    }

    [HttpGet("latest")] // /api/blogs/latest?count=5 wegen FromQuery kann man es anpassen 
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLatestBlogs([FromQuery] int count = 3)
    {
        var result = await _latestBlogsHandler.Handle(count);
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]  
    public async Task<IActionResult> CreateContent([FromBody] CreateBlogRequest request)
    {
        Guid? userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty;


        if (!userId.HasValue)
            return Unauthorized(new { message = "User nicht authentifiziert." });

        var result = await _createContentHandler.Handle(request, userId.Value);
        return CreatedAtAction(nameof(GetBlogDetails), new { slug = result.Slug }, result);
    }

    [HttpPatch("{blogId}/view")]
    [ProducesResponseType(typeof(UpdateViewsContentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IncrementBlogViews(int blogId)
    {
        var result = await _updateViewsBlogHandler.Handle(blogId);
        return Ok(result);
    }

}