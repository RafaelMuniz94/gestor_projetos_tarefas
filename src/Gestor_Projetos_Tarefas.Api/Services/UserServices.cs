using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;

namespace Gestor_Projetos_Tarefas.Api.Services
{
    public class UserServices: IUserServices
    {

        private readonly IUsersRepository usersRepository;
        private readonly IProjectsRepository projectsRepository;
        private readonly ITasksRepository tasksRepository;

        public UserServices(IProjectsRepository projectsRepository, IUsersRepository usersRepository, ITasksRepository tasksRepository)
        {
            this.usersRepository = usersRepository;
            this.projectsRepository = projectsRepository;
            this.tasksRepository = tasksRepository;
        }

        public async Task<bool> ChangeTaskUser(Guid oldUserID, Guid newUserID, Guid projectID)
        {

            Project project = await projectsRepository.ReturnProject(projectID);
            if(project == null) return false;

            bool? oldUserRemoved = await usersRepository.RemoveProjectFromUser(oldUserID, projectID);
            
            if (oldUserRemoved == null) return false;

            bool newUserAdded = await usersRepository.AddProject(newUserID, project);

            return (bool)oldUserRemoved && newUserAdded;
        }

        public async Task<User> ReturnUser(Guid userID)
        {
            User user = await usersRepository.ReturnUser(userID);
            List<ProjectTask> tasks = await tasksRepository.ReturnTasktListByUser(userID);

           if(tasks.Count() > 0) {

                List<Guid> projects = tasks.Where(task => task.User == userID && task.Status != ProjectTaskStatus.Concluida).Select(field => field.Project).ToList();

                user.Projects = await projectsRepository.ReturnProjectList(projects);
            }

            return user;
        }
    }
}
