using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Database.Interfaces
{
    public interface ITasksRepository
    {
        Task<ProjectTask> AddTask(ProjectTask task);
        Task<ProjectTask> UpdateTask(UpdateProjectTaskDTO taskDTO);
        Task<ProjectTask> ReturnTask(Guid taskID); 
        Task<List<ProjectTask>> ReturnTasktListByProject(Guid projectID);
        Task<bool> ReturnActiveTasktByProject(Guid projectID);
        Task<List<ProjectTask>> ReturnTasktListByUser(Guid userID);
        Task<bool?> DeleteTask(Guid taskID);
    }
}
