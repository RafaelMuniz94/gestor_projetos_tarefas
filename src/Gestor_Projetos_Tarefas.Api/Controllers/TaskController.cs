using AutoMapper;
using Gestor_Projetos_Tarefas.Api.ViewModels;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Database.Repositories;
using Gestor_Projetos_Tarefas.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly IProjectsRepository projectsRepository;
        private readonly ITasksRepository tasksRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IHistoryUpdateProjectRepository historyRepository;
        private readonly IMapper mapper;

        public TaskController(IProjectsRepository _projectsRepository, ITasksRepository _tasksRepository, IUsersRepository _usersRepository, IHistoryUpdateProjectRepository _historyRepository, IMapper _mapper)
        {
            this.projectsRepository = _projectsRepository;
            this.tasksRepository = _tasksRepository;
            this.usersRepository = _usersRepository;
            this.historyRepository = _historyRepository;
            this.mapper = _mapper;
        }

        [HttpPost("{projectID}")]
        public async Task<IActionResult> CreateTask(Guid projectID, [FromBody] CreateTaskViewModel createTaskViewModel)
        {
            Project project = await projectsRepository.ReturnProject(projectID);

            if(project == null) {
                return NotFound("Projeto não encontrado, não é possível adicionar uma tarefa!");
            }

            User user = await usersRepository.ReturnUser(createTaskViewModel.User);
            
            if (user == null)
            {
                return NotFound("Usuário não encontrado, não é possível adicionar uma tarefa!");
            }

            ProjectTask task = new ProjectTask(createTaskViewModel.TaskStatus,
                createTaskViewModel.TaskPriority,
                createTaskViewModel.TaskTitle,
                createTaskViewModel.TaskDescription,
                createTaskViewModel.TaskExpirationDate,
                projectID,
                user.ID);

            ProjectTask createdTask = await tasksRepository.AddTask(task);

            if (createdTask == null)
            {
                return BadRequest("Não foi possível cadastrar uma nova tarefa!");
            }

            TaskUpdateHistory record = new TaskUpdateHistory(user.ID, "Criacao", createdTask.ID, createTaskViewModel.Comment);
            bool historySuccess = await historyRepository.AddHistoryRecord(record);

            if (!historySuccess) { 
            // TODO: Adicionar LOG
            }

            return Ok(createdTask);
        }

        [HttpPut("{taskID}")]
        public async Task<IActionResult> UpdateTask(Guid taskID)
        {
            return Ok();
        }

        [HttpDelete("{taskID}")]
        public async Task<IActionResult> DeleteTask(Guid taskID)
        {
            return Ok();
        }
    }
}
