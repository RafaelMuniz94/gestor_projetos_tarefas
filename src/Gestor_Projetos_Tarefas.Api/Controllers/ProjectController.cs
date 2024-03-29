﻿using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Api.Utils;
using Gestor_Projetos_Tarefas.Api.ViewModels.Request;
using Gestor_Projetos_Tarefas.Api.ViewModels.Return;
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
        private readonly IUserServices usersServices;

        public ProjectController(IProjectsRepository _projectsRepository, ITasksRepository _tasksRepository, IUserServices _usersServices)
        {
            this.projectsRepository = _projectsRepository;
            this.tasksRepository = _tasksRepository;
            this.usersServices = _usersServices;
        }

        [HttpGet]
        public async Task<IActionResult> ReturnProjects()
        {
            List<Project> projects = await projectsRepository.ReturnProjectList();

            return Ok(projects);
        }

        [HttpGet("user/{userID}")]
        public async Task<IActionResult> ReturnProjectsByUser(Guid userID)
        {
            User user = await usersServices.ReturnUser(userID);

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

            ProjectDetailsViewModel returnViewModel = new ProjectDetailsViewModel();
            returnViewModel.project = project;
            returnViewModel.tasks = tasks;


            return Ok(returnViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel createProjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = new ErrorHandlingUtils().ReturnModelStateMessages(ModelState);
                return BadRequest(errorMessage);
            }

            Project project = new Project(createProjectViewModel.ProjectName, createProjectViewModel.ProjectDescription);

            project = await projectsRepository.AddProject(project);

            if(project == null)
            {
                return BadRequest("Não foi possível cadastrar um novo projeto!");
            }

            return Created($"api/project/{project.ID}",project);
        }

    }
}
