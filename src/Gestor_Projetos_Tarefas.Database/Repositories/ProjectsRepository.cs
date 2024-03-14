using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Database.Context;
using Microsoft.EntityFrameworkCore;
using Gestor_Projetos_Tarefas.Domain.DTOs;

namespace Gestor_Projetos_Tarefas.Database.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly DataBaseContext dbContext;

        public ProjectsRepository(DataBaseContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Project> AddProject(Project project)
        {

            await dbContext.Projects.AddAsync(project);
            int dbResponse = await dbContext.SaveChangesAsync();

            return dbResponse > 0 ? project : null;  // Se salvou mais de uma requisicao, sera possivel assumir que a operacao deu certo

        }

        public async Task<bool?> DeleteProject(Guid projectID)
        {
            Project project = await dbContext.Projects.Where(project => project.ID == projectID).SingleOrDefaultAsync();

            if (project == null) return null;

            dbContext.Projects.Remove(project);
            int dbResponse = await dbContext.SaveChangesAsync();

            return dbResponse > 0; // Se salvou mais de uma requisicao, sera possivel assumir que a operacao deu certo


        }

        public async Task<Project> ReturnProject(Guid projectID)
        {
            Project project = await dbContext.Projects.SingleOrDefaultAsync(project => project.ID == projectID);

            if (project == null) return null;

            return project;
        }

        public async Task<List<Project>> ReturnProjectList(List<Guid> projectIDs)
        {
            List<Project> list = await dbContext.Projects.Where(project => projectIDs.All(ids => project.ID == ids)).ToListAsync();

            return list;
        }

        public async Task<Project> UpdateProject(UpdateProjectDTO projectDTO)
        {
            Project updateProject = await dbContext.Projects.SingleOrDefaultAsync(project => project.ID == projectDTO.ID);

            if(updateProject is not null)
            {
                updateProject.Name = projectDTO.Name ?? updateProject.Name;
                updateProject.Description = projectDTO.Description ?? updateProject.Description;
                await dbContext.SaveChangesAsync();
            }

            return updateProject;
        }
    }
}
