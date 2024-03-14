using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;


namespace Gestor_Projetos_Tarefas.Database.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly DataBaseContext dbContext;

        public TasksRepository(DataBaseContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<ProjectTask> AddTask(ProjectTask task)
        {
            await dbContext.Tasks.AddAsync(task);
            int dbResponse = await dbContext.SaveChangesAsync();

            return dbResponse > 0 ? task : null;  
        }

        public async Task<bool?> DeleteTask(Guid taskID)
        {
           ProjectTask task = await dbContext.Tasks.Where(task => task.ID == taskID).SingleOrDefaultAsync();

            if (task == null) return null;

            dbContext.Tasks.Remove(task);
            int dbResponse = await dbContext.SaveChangesAsync();

            return dbResponse > 0; 
        }

        public async Task<List<ProjectTask>> ReturnTasktListByProject(Guid projectID)
        {
            List<ProjectTask> tasks = await dbContext.Tasks.Where(task => task.Project == projectID).ToListAsync();
            return tasks;
        }

        public async Task<List<ProjectTask>> ReturnTasktListByUser(Guid userID)
        {
            List<ProjectTask> tasks = await dbContext.Tasks.Where(task => task.User == userID).ToListAsync();
            return tasks;
        }

        public async Task<ProjectTask> UpdateTask(UpdateProjectTaskDTO taskDTO)
        {
            ProjectTask updatedTask  = await dbContext.Tasks.Where(task => task.ID == taskDTO.ID).SingleOrDefaultAsync();
            
            if (updatedTask is not null)
            {
                updatedTask.Status = taskDTO.Status ?? updatedTask.Status; 
                updatedTask.Title = taskDTO.Title ?? updatedTask.Title; 
                updatedTask.Description = taskDTO.Description ?? updatedTask.Description; 
                updatedTask.ExpirationDate = taskDTO.ExpirationDate ?? updatedTask.ExpirationDate; 
                updatedTask.Project = taskDTO.Project ?? updatedTask.Project; 
                updatedTask.User = taskDTO.User ?? updatedTask.User; 


                await dbContext.SaveChangesAsync();
            }

            return updatedTask;
        }


        public async Task<bool> ReturnActiveTasktByProject(Guid projectID)
        {
            List<ProjectTask> tasks = await dbContext.Tasks.Where(task => task.Project == projectID && task.Status == ProjectTaskStatus.Pendente).ToListAsync();
            return tasks.Count > 0;
        }
    }
}
