using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Interactions;
public class UpdateViewsBlogHandler : ICommandHandler<UpdateBlogViewsCommand, UpdateViewsContentResponse>
{
    private readonly IBlogInterface _repository;
    public UpdateViewsBlogHandler(IBlogInterface repository)
    {
        _repository = repository;
    }

    public async Task<UpdateViewsContentResponse> HandleAsync(UpdateBlogViewsCommand command, CancellationToken cancellationToken = default)
    {
        var blog = await _repository.GetBlogByIdAsync(command.BlogId, cancellationToken);

        if (blog == null)
            throw new ArgumentException($"Blog mit ID {command.BlogId} nicht gefunden.");

        blog.Views += 1;

        var updatedBlog = await _repository.UpdateBlogViewsAsync(blog, cancellationToken);

        return new UpdateViewsContentResponse(
            updatedBlog.Id, 
            updatedBlog.Views
        );
    }

}