using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Database.Interfaces
{
    internal interface IProjectsRepository
    {
        Task<Project> AddProject(Project project);
        Task<Project> ReturnProject(Guid projectID);
        Task<List<Project>> ReturnProjectList(List<Guid> projectIDs);
        Task<bool?> DeleteProject(Guid projectID);
        Task<Project> UpdateProject(UpdateProjectDTO projectDTO);
    }
}
