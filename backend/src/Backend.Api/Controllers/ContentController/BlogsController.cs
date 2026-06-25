using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.Content;
using Backend.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Api.Helpers; 
using Backend.Application.Common.Interfaces; 


namespace Backend.Api.Controllers;

[ApiController]
[Route("api/blogs")] // /api/blogs
public class BlogsController : ControllerBase
{
    private readonly IQueryHandler<GetAllBlogsQuery, IEnumerable<ContentListResponse>> _getAllBlogsHandler;
    private readonly IQueryHandler<GetBlogsDetailsQuery, ContentDetailResponse> _blogDetailshandler;
    private readonly ICommandHandler<CreateContentCommand, ContentDetailResponse> _createContentHandler;
    private readonly IQueryHandler<GetLatestBlogsQuery, IEnumerable<ContentListResponse>> _latestBlogsHandler;

    private readonly ICommandHandler<UpdateBlogViewsCommand, UpdateViewsContentResponse> _updateViewsBlogHandler;


    // Handler Dependency Injection 
    public BlogsController(
        IQueryHandler<GetAllBlogsQuery, IEnumerable<ContentListResponse>> getAllBlogsHandler,
        IQueryHandler<GetBlogsDetailsQuery, ContentDetailResponse> blogDetailshandler,
        ICommandHandler<CreateContentCommand, ContentDetailResponse> createContentHandler,
        IQueryHandler<GetLatestBlogsQuery, IEnumerable<ContentListResponse>> latestBlogsHandler, 
        ICommandHandler<UpdateBlogViewsCommand, UpdateViewsContentResponse> updateViewsBlogHandler
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
    public async Task<IActionResult> GetAllBlogs(CancellationToken ct = default)
    {
        var result = await _getAllBlogsHandler.HandleAsync(new GetAllBlogsQuery(CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty), ct);

        return Ok(result); 
    }

    [HttpGet("{slug}")] // /api/blogs/123
    [ProducesResponseType(typeof(ContentDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlogDetails(string slug, CancellationToken ct = default)
    {
        Guid userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User) ?? Guid.Empty;

        var result = await _blogDetailshandler.HandleAsync(new GetBlogsDetailsQuery(slug, userId), ct);

        if (result == null)
        {
            return NotFound(new { message = "Blog nicht gefunden." });
        }

        return Ok(result);
    }

    [HttpGet("latest")] // /api/blogs/latest?count=5 wegen FromQuery kann man es anpassen 
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLatestBlogs([FromQuery] int count = 3, CancellationToken ct = default)
    {
        var result = await _latestBlogsHandler.HandleAsync(new GetLatestBlogsQuery(count), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]  
    public async Task<IActionResult> CreateContent([FromBody] CreateContentRequest request, CancellationToken ct)
    {
        Guid? userId = CurrentUserHelper.GetCurrentUserIdFromClaims(User);

        if (userId == null || userId == Guid.Empty)
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

        return CreatedAtAction(nameof(GetBlogDetails), new { slug = result.Slug }, result);
    }

    [HttpPatch("{blogId}/view")]
    [ProducesResponseType(typeof(UpdateViewsContentResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IncrementBlogViews(
        [FromRoute] int blogId, 
        [FromQuery] string slug, 
        CancellationToken cancellationToken)
    {
        var result = await _updateViewsBlogHandler.HandleAsync(new UpdateBlogViewsCommand(blogId, slug), cancellationToken);

        return Ok(result);
    }

}