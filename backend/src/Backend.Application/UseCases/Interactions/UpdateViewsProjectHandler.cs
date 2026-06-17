using Backend.Domain.Contracts;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases.Interactions;
public class UpdateViewsProjectHandler
{
    private readonly IProjectInterface _repository;
    public UpdateViewsProjectHandler(IProjectInterface repository)
    {
        _repository = repository;
    }

    public async Task<UpdateViewsContentResponse>  Handle(int projectId)
    {
        var project = await _repository.GetProjectByIdAsync(projectId);

        if (project == null)
            throw new ArgumentException($"Projekt mit ID {projectId} nicht gefunden.");

        project.Views += 1;

        var updatedProject = await _repository.UpdateProjectViewsAsync(project);

        return new UpdateViewsContentResponse(
            updatedProject.Id, 
            updatedProject.Views
        );
    }
    
}