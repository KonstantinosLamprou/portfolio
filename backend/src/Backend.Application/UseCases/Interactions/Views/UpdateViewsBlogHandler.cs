using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;
public class UpdateViewsBlogHandler
{
    private readonly IBlogInterface _repository;
    public UpdateViewsBlogHandler(IBlogInterface repository)
    {
        _repository = repository;
    }

    public async Task<UpdateViewsContentResponse> Handle(int blogId)
    {
        var blog = await _repository.GetBlogByIdAsync(blogId);

        if (blog == null)
            throw new ArgumentException($"Blog mit ID {blogId} nicht gefunden.");

        blog.Views += 1;

        var updatedBlog = await _repository.UpdateBlogViewsAsync(blog);

        return new UpdateViewsContentResponse(
            updatedBlog.Id, 
            updatedBlog.Views
        );
    }

}