using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.UseCases.Interactions;
public class UpdateViewsProjectHandler : ICommandHandler<UpdateProjectViewsCommand, UpdateViewsContentResponse>
{
    private readonly IProjectInterface _repository;
    public UpdateViewsProjectHandler(IProjectInterface repository)
    {
        _repository = repository;
    }

    public async Task<UpdateViewsContentResponse> HandleAsync(UpdateProjectViewsCommand command, CancellationToken cancellationToken = default)
    {
        var project = await _repository.GetProjectByIdAsync(command.ProjectId, cancellationToken);

        if (project == null)
            throw new ArgumentException($"Projekt mit ID {command.ProjectId} nicht gefunden.");

        project.Views += 1;

        var updatedProject = await _repository.UpdateProjectViewsAsync(project, cancellationToken);

        return new UpdateViewsContentResponse(
            updatedProject.Id, 
            updatedProject.Views
        );
    }
    
}