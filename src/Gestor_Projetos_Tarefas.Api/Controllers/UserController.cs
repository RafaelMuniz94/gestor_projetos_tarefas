﻿using AutoMapper;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Projetos_Tarefas.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IProjectsRepository projectsRepository;
        private readonly ITasksRepository tasksRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IHistoryUpdateProjectRepository historyRepository;


        public UserController(IProjectsRepository _projectsRepository, ITasksRepository _tasksRepository, IUsersRepository _usersRepository, IHistoryUpdateProjectRepository _historyRepository)
        {
            this.projectsRepository = _projectsRepository;
            this.tasksRepository = _tasksRepository;
            this.usersRepository = _usersRepository;
            this.historyRepository = _historyRepository;
        }

        [HttpDelete("{userID}/{projectID}")]
        public async Task<IActionResult> RemoveProject(Guid userID, Guid projectID)
        {
            Project project = await projectsRepository.ReturnProject(projectID);
            if (project == null)
            {
                return NotFound("Projeto não encontrado!");
            }

            List<ProjectTask> tasks = await tasksRepository.ReturnTasktListByProject(projectID);
            if(tasks.Count > 0)
            {
                List<ProjectTask> userTasks = tasks.Where(task => task.User == userID).ToList();
                bool openedTasks = userTasks.Any(userTask => userTask.Status != ProjectTaskStatus.Concluida);

                if(openedTasks)
                {
                    throw new Exception("Nao sera possivel remover a tarefa, existem tarefas nao concluidas para o usuario, favor finalizar!");
                }
            }

            bool? deleteStatus = await usersRepository.RemoveProjectFromUser(userID,projectID);

            if (deleteStatus == null)
            {
                return NotFound("O projeto ou usuario nao foram encontrados!");
            }

            if (!(bool)deleteStatus)
            {
                throw new Exception("Nao foi possivel remover o projeto!");
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePerformanceReport(Guid userID,Guid userIDReport)
        {
            User requester = await usersRepository.ReturnUser(userID);
            
            if(requester != null && requester.Role != Role.Gerente)
            {
                throw new Exception("O usuario nao possui permissao para gerar relatorio!");
            }

            List<ProjectTask> userTasks = await tasksRepository.ReturnTasktListByUser(userID);

            int userTasksTotal = userTasks.Count;
            if (userTasksTotal == 0)
            {
                return BadRequest($"O Usuario {userIDReport} nao possui tarefas");
            }

            List<Guid> completeTaks = await historyRepository.ReturnHistoryByUser(userIDReport);
            int completeTaksTotal = completeTaks.Count;

            if (completeTaksTotal == 0)
            {
                return Ok($"O Usuario {userIDReport} nao finalizou tarefas nos ultimos 30 dias.");
            }

            decimal tasksMedia = completeTaksTotal / userTasksTotal;

            return Ok($"O usuario {userIDReport} realizou {completeTaksTotal} das {userTasksTotal}, totalizando um total de {tasksMedia}% de finalizacoes");
        }
    }
}
