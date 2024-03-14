using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models;


namespace Gestor_Projetos_Tarefas.Database.Interfaces
{
    public interface IProjectsRepository
    {
        Task<Project> AddProject(Project project);
        Task<Project> ReturnProject(Guid projectID);
        Task<List<Project>> ReturnProjectList(List<Guid> projectIDs);
        Task<bool?> DeleteProject(Guid projectID);
        Task<Project> UpdateProject(UpdateProjectDTO projectDTO);
    }
}
