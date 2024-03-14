using AutoMapper;
using Gestor_Projetos_Tarefas.Api.ViewModels;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectsRepository projectsRepository;
        private readonly ITasksRepository  tasksRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public ProjectController(IProjectsRepository _projectsRepository, ITasksRepository _tasksRepository, IUsersRepository _usersRepository, IMapper _mapper)
        {
            this.projectsRepository = _projectsRepository;
            this.tasksRepository = _tasksRepository;
            this.usersRepository = _usersRepository;
            this.mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ReturnProjects()
        {
            List<Project> projects = await projectsRepository.ReturnProjectList();

            return Ok(projects);
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> ReturnProjectsByUser(Guid userID)
        {
            User user = await usersRepository.ReturnUser(userID);

            if(user == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            return Ok(user.Projects);
        }

        [HttpGet("{projectID}")]
        public async Task<IActionResult> ReturnProjectDetail(Guid projectID)
        {
            Project project = await projectsRepository.ReturnProject(projectID);
            if(project == null)
            {
                return NotFound("Projeto não encontrado!");
            }

            List<ProjectTask> tasks = await tasksRepository.ReturnTasktListByProject(projectID);


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel createProjectViewModel)
        {
            Project project = new Project(createProjectViewModel.ProjectName, createProjectViewModel.ProjectDescription);

            project = await projectsRepository.AddProject(project);

            if(project == null)
            {
                return BadRequest("Não foi possível cadastrar um novo projeto!");
            }

            return Ok(project);
        }

    }
}
