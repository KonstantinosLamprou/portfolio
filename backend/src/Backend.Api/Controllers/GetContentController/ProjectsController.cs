using Backend.Domain.Contracts;
using Backend.Application.UseCases.GetContent;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers.GetContentController;

[ApiController]
[Route("api/projects")] // /api/blogs
public class ProjectsController : ControllerBase
{
    private readonly GetAllProjectsHandler _handler; 

    public ProjectsController(GetAllProjectsHandler handler)
    {
        _handler = handler; 
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContentListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProjects()
    {
        var result = await _handler.Handle();

        return Ok(result);
    }

}
