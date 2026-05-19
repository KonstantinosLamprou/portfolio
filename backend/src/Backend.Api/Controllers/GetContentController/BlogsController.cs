using Backend.Domain.Contracts;
using Backend.Application.UseCases.GetContent;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/blogs")] // /api/blogs
public class BlogsController : ControllerBase
{
    private readonly GetAllBlogsHandler _getAllBlogsHandler;

    // Handler Dependency Injection 
    public BlogsController(GetAllBlogsHandler getAllBlogsHandler)
    {
        _getAllBlogsHandler = getAllBlogsHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBlogs()
    {
        var result = await _getAllBlogsHandler.Handle();

        return Ok(result); 
    }
}