using AutoMapper;

using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Api.Utils;
using Gestor_Projetos_Tarefas.Api.ViewModels.Request;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.DTOs;
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
        private readonly IUserServices usersServices;
        private readonly IMapper mapper;

        public TaskController(IProjectsRepository _projectsRepository, ITasksRepository _tasksRepository, IUsersRepository _usersRepository, IHistoryUpdateProjectRepository _historyRepository, IUserServices _usersServices, IMapper _mapper)
        {
            this.projectsRepository = _projectsRepository;
            this.tasksRepository = _tasksRepository;
            this.usersRepository = _usersRepository;
            this.historyRepository = _historyRepository;
            this.usersServices = _usersServices;
            this.mapper = _mapper;
        }

        [HttpPost("{projectID}")]
        public async Task<IActionResult> CreateTask(Guid projectID, [FromBody] CreateTaskViewModel createTaskViewModel)
        {
            if (!ModelState.IsValid)
            {
               string errorMessage = new ErrorHandlingUtils().ReturnModelStateMessages(ModelState);
                return BadRequest(errorMessage);
            }

            Project project = await projectsRepository.ReturnProject(projectID);

            if(project == null) {
                return NotFound("Projeto não encontrado, não é possível adicionar uma tarefa!");
            }

            List<ProjectTask> projectTasks = await tasksRepository.ReturnTasktListByProject(project.ID);

            if(projectTasks.Count == 20)
            {
                return BadRequest("O Projeto ja possui o limite de 20 tarefas, finalize ou remova tarefas para adicionar novas!");
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


            bool userSaved = await usersRepository.AddProject(user.ID, project);

            if(!userSaved)
            {
                // TODO: LOG 
            }
            
            RecordOperation("Criar", createdTask.ID,createdTask.User, createTaskViewModel.Comment);

            return Created($"api/task/{createdTask.ID}", createdTask);
        }

        [HttpPut("{taskID}")]
        public async Task<IActionResult> UpdateTask(Guid taskID, [FromBody] UpdateTaskViewModel updateTaskViewModel)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = new ErrorHandlingUtils().ReturnModelStateMessages(ModelState);
                return BadRequest(errorMessage);
            }

            UpdateProjectTaskDTO taskDto = mapper.Map<UpdateProjectTaskDTO>(updateTaskViewModel);
            taskDto.ID = taskID;

            ProjectTask oldTask = await tasksRepository.ReturnTask(taskID);

            ProjectTask updatedTask = await tasksRepository.UpdateTask(taskDto);

            if(updatedTask == null)
            {
                return NotFound("A tarefa nao foi encontrada!");
            }

               
                if(oldTask.User != updatedTask.User) {
                    bool updatedUser = await usersServices.ChangeTaskUser(oldTask.User, updatedTask.User, updatedTask.Project);
                    if(!updatedUser)
                    {
                        throw new Exception("Nao foi possivel realizar a troca de usuarios");
                    }
                }
            

            RecordOperation("Atualizar", updatedTask.ID,updatedTask.User, updateTaskViewModel.Comment);

            return Ok(updatedTask);
        }

        [HttpDelete("{taskID}")]
        public async Task<IActionResult> DeleteTask(Guid taskID, [FromBody] DeleteTaskViewModel deleTaskViewModel)
        {
            if (!ModelState.IsValid)
            {
                string errorMessage = new ErrorHandlingUtils().ReturnModelStateMessages(ModelState);
                return BadRequest(errorMessage);
            }

            bool? deleteStatus = await tasksRepository.DeleteTask(taskID);

            if(deleteStatus == null)
            {
                return NotFound("A tarefa nao foi encontrada!");
            }

            if (!(bool)deleteStatus)
            {
                throw new Exception("Nao foi possivel deletar a tarefa!"); 
            }

            RecordOperation("Deletar", taskID, deleTaskViewModel.User, deleTaskViewModel.Comment);
            return NoContent();
        }


        private async void RecordOperation(string operation, Guid taskID,Guid UserID, string? comment)
        {
            TaskUpdateHistory record = new TaskUpdateHistory(UserID, operation, taskID, comment);
            bool historySuccess = await historyRepository.AddHistoryRecord(record);

            if (!historySuccess)
            {
                // TODO: Adicionar LOG
            }
        }
    }
}
