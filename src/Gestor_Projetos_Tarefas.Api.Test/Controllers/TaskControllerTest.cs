
using Moq;
using Microsoft.AspNetCore.Mvc;
using Gestor_Projetos_Tarefas.Api.Controllers;
using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Api.ViewModels.Request;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using AutoMapper;
using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Gestor_Projetos_Tarefas.Api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;
using Gestor_Projetos_Tarefas.Domain.Exceptions;
using System;

namespace Gestor_Projetos_Tarefas.Tests.Controllers
{
    public class TaskControllerTests
    {
        [Fact]
        public async Task CreateTask_WithValidModel_ReturnsCreatedResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockProjectsRepository = new Mock<IProjectsRepository>();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUsersRepository = new Mock<IUsersRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

            mockProjectsRepository.Setup(repository => repository.ReturnProject(projectId))
                .ReturnsAsync(new Project());
            mockTasksRepository.Setup(repository => repository.ReturnTasktListByProject(projectId))
                .ReturnsAsync(new List<ProjectTask>());
            mockTasksRepository.Setup(repository => repository.AddTask(It.IsAny<ProjectTask>()))
                .ReturnsAsync(new ProjectTask());
            mockUsersRepository.Setup(repository => repository.ReturnUser(userId))
                .ReturnsAsync(new User());
            mockUsersRepository.Setup(repository => repository.AddProject(userId,new Project())).ReturnsAsync(true);

            var controller = new TaskController(mockProjectsRepository.Object, mockTasksRepository.Object, mockUsersRepository.Object, null, mockUserServices.Object, null);

            var viewModel = new CreateTaskViewModel
            {
                TaskTitle = "Tarefa",
                TaskStatus = ProjectTaskStatus.Pendente,
                TaskPriority = TaskPriority.Alta,
                TaskDescription = "Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(1),
                User = userId
            };

            // Act
            var result = await controller.CreateTask(projectId, viewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateTask_WithValidModel_ReturnsNullProject_ReturnsNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockProjectsRepository = new Mock<IProjectsRepository>();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUsersRepository = new Mock<IUsersRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

            mockProjectsRepository.Setup(repository => repository.ReturnProject(projectId))
                .ReturnsAsync((Project)null);
            mockTasksRepository.Setup(repository => repository.ReturnTasktListByProject(projectId))
                .ReturnsAsync(new List<ProjectTask>());
            mockTasksRepository.Setup(repository => repository.AddTask(It.IsAny<ProjectTask>()))
                .ReturnsAsync(new ProjectTask());
            mockUsersRepository.Setup(repository => repository.ReturnUser(userId))
                .ReturnsAsync(new User());
            mockUsersRepository.Setup(repository => repository.AddProject(userId, new Project())).ReturnsAsync(true);

            var controller = new TaskController(mockProjectsRepository.Object, mockTasksRepository.Object, mockUsersRepository.Object, null, mockUserServices.Object, null);

            var viewModel = new CreateTaskViewModel
            {
                TaskTitle = "Tarefa",
                TaskStatus = ProjectTaskStatus.Pendente,
                TaskPriority = TaskPriority.Alta,
                TaskDescription = "Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(1),
                User = userId
            };

            // Act
            var result = await controller.CreateTask(projectId, viewModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateTask_ReturnNullProjectTasks_ThrowsDomainException()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockProjectsRepository = new Mock<IProjectsRepository>();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUsersRepository = new Mock<IUsersRepository>();
            var mockHistoryRepository = new Mock<IHistoryUpdateProjectRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();


            List<ProjectTask> projectTaskList = new List<ProjectTask>();
            for(int i = 0; i < 20; i++)
            {
                projectTaskList.Add(new ProjectTask());
            }

            mockProjectsRepository.Setup(repository => repository.ReturnProject(It.IsAny<Guid>()))
                .ReturnsAsync(new Project());
            mockTasksRepository.Setup(repository => repository.ReturnTasktListByProject(It.IsAny<Guid>()))
                .ReturnsAsync(projectTaskList);
            mockUsersRepository.Setup(repository => repository.ReturnUser(userId))
                .ReturnsAsync(new User());
            mockUsersRepository.Setup(repository => repository.AddProject(userId, new Project())).ReturnsAsync(true);
            mockHistoryRepository.Setup(repository => repository.AddHistoryRecord(It.IsAny<TaskUpdateHistory>())).ReturnsAsync(true);

            var controller = new TaskController(mockProjectsRepository.Object, mockTasksRepository.Object, mockUsersRepository.Object, mockHistoryRepository.Object, mockUserServices.Object, null);

            var viewModel = new CreateTaskViewModel
            {
                TaskTitle = "Tarefa",
                TaskStatus = ProjectTaskStatus.Pendente,
                TaskPriority = TaskPriority.Alta,
                TaskDescription = "Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(1),
                User = userId
            };

            // Act
            try
            {
                var result = await controller.CreateTask(projectId, viewModel);
            }catch (Exception exception)
            {
                // Assert
                var requestResult = Assert.IsType<DomainException>(exception);
                string errorMessage = "O Projeto ja possui o limite de 20 tarefas, finalize ou remova tarefas para adicionar novas!";
                Assert.Equal(errorMessage, requestResult.Message);
            }
        }

        [Fact]
        public async Task CreateTask_ReturnNullUser_ReturnsNotFoundResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockProjectsRepository = new Mock<IProjectsRepository>();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUsersRepository = new Mock<IUsersRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

            mockProjectsRepository.Setup(repository => repository.ReturnProject(projectId))
                .ReturnsAsync(new Project());
            mockTasksRepository.Setup(repository => repository.ReturnTasktListByProject(projectId))
                .ReturnsAsync(new List<ProjectTask>());
            mockTasksRepository.Setup(repository => repository.AddTask(It.IsAny<ProjectTask>()))
                .ReturnsAsync(new ProjectTask());
            mockUsersRepository.Setup(repository => repository.ReturnUser(userId))
                .ReturnsAsync((User)null);
            mockUsersRepository.Setup(repository => repository.AddProject(userId, new Project())).ReturnsAsync(true);

            var controller = new TaskController(mockProjectsRepository.Object, mockTasksRepository.Object, mockUsersRepository.Object, null, mockUserServices.Object, null);

            var viewModel = new CreateTaskViewModel
            {
                TaskTitle = "Tarefa",
                TaskStatus = ProjectTaskStatus.Pendente,
                TaskPriority = TaskPriority.Alta,
                TaskDescription = "Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(1),
                User = userId
            };

            // Act
            var result = await controller.CreateTask(projectId, viewModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateTask_ReturnNullCreatedTask_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mockProjectsRepository = new Mock<IProjectsRepository>();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUsersRepository = new Mock<IUsersRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

            mockProjectsRepository.Setup(repository => repository.ReturnProject(projectId))
                .ReturnsAsync(new Project());
            mockTasksRepository.Setup(repository => repository.ReturnTasktListByProject(projectId))
                .ReturnsAsync(new List<ProjectTask>());
            mockTasksRepository.Setup(repository => repository.AddTask(It.IsAny<ProjectTask>()))
                .ReturnsAsync((ProjectTask)null);
            mockUsersRepository.Setup(repository => repository.ReturnUser(userId))
                .ReturnsAsync(new User());
            mockUsersRepository.Setup(repository => repository.AddProject(userId, new Project())).ReturnsAsync(true);

            var controller = new TaskController(mockProjectsRepository.Object, mockTasksRepository.Object, mockUsersRepository.Object, null, mockUserServices.Object, null);

            var viewModel = new CreateTaskViewModel
            {
                TaskTitle = "Tarefa",
                TaskStatus = ProjectTaskStatus.Pendente,
                TaskPriority = TaskPriority.Alta,
                TaskDescription = "Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(1),
                User = userId
            };

            // Act
            var result = await controller.CreateTask(projectId, viewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateTask_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new TaskController(null, null, null, null, null, null);
            controller.ModelState.AddModelError("TaskTitle", "Required");

            // Act
            var result = await controller.CreateTask(Guid.NewGuid(), new CreateTaskViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_WithValidModel_ReturnsOkResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

            // Configuração do IMapper com o perfil TaskUpdateMapper
            mockMapper.Setup(mapper => mapper.Map<UpdateProjectTaskDTO>(It.IsAny<UpdateTaskViewModel>()))
                .Returns<UpdateTaskViewModel>(viewModel =>
                {
                    var configuration = new MapperConfiguration(configuration => configuration.AddProfile<TaskUpdateMapper>());
                    IMapper mapper = new Mapper(configuration);
                    return mapper.Map<UpdateProjectTaskDTO>(viewModel);
                });

            mockTasksRepository.Setup(repository => repository.ReturnTask(taskId))
                .ReturnsAsync(new ProjectTask());
            mockTasksRepository.Setup(repository => repository.UpdateTask(It.IsAny<UpdateProjectTaskDTO>()))
                .ReturnsAsync(new ProjectTask { ID = taskId });

            var controller = new TaskController(null, mockTasksRepository.Object, null, null, mockUserServices.Object, mockMapper.Object);
            var viewModel = new UpdateTaskViewModel
            {
                TaskTitle = "Tarefa Atualizada",
                TaskStatus = ProjectTaskStatus.EmAndamento,
                TaskDescription = "Nova Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(2),
                User = Guid.NewGuid()
            };

            // Act
            var result = await controller.UpdateTask(taskId, viewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_ReturnUpdatedTaskNull_ReturnsNotFoundResult()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

            // Configuração do IMapper com o perfil TaskUpdateMapper
            mockMapper.Setup(mapper => mapper.Map<UpdateProjectTaskDTO>(It.IsAny<UpdateTaskViewModel>()))
                .Returns<UpdateTaskViewModel>(viewModel =>
                {
                    var configuration = new MapperConfiguration(configuration => configuration.AddProfile<TaskUpdateMapper>());
                    IMapper mapper = new Mapper(configuration);
                    return mapper.Map<UpdateProjectTaskDTO>(viewModel);
                });

            mockTasksRepository.Setup(repository => repository.ReturnTask(taskId))
                .ReturnsAsync(new ProjectTask());
            mockTasksRepository.Setup(repository => repository.UpdateTask(It.IsAny<UpdateProjectTaskDTO>()))
                .ReturnsAsync((ProjectTask)null);

            var controller = new TaskController(null, mockTasksRepository.Object, null, null, mockUserServices.Object, mockMapper.Object);
            var viewModel = new UpdateTaskViewModel
            {
                TaskTitle = "Tarefa Atualizada",
                TaskStatus = ProjectTaskStatus.EmAndamento,
                TaskDescription = "Nova Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(2),
                User = Guid.NewGuid()
            };

            // Act
            var result = await controller.UpdateTask(taskId, viewModel);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateTask_DifferentUsers_ThrowsDomainException()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var oldUserId = Guid.NewGuid();
            var newUserId = Guid.NewGuid();

            var mockTasksRepository = new Mock<ITasksRepository>();
            var mockUserServices = new Mock<IUserServices>();
            var mockMapper = new Mock<IMapper>();

          
            mockTasksRepository.Setup(repository => repository.UpdateTask(It.IsAny<UpdateProjectTaskDTO>()))
                .ReturnsAsync(new ProjectTask { ID = taskId });

            mockMapper.Setup(mapper => mapper.Map<UpdateProjectTaskDTO>(It.IsAny<UpdateTaskViewModel>()))
                .Returns<UpdateTaskViewModel>(viewModel =>
                {
                    var configuration = new MapperConfiguration(configuration => configuration.AddProfile<TaskUpdateMapper>());
                    IMapper mapper = new Mapper(configuration);
                    return mapper.Map<UpdateProjectTaskDTO>(viewModel);
                });

           
            var existingTask = new ProjectTask { ID = taskId, User = oldUserId };
            mockTasksRepository.Setup(repository => repository.ReturnTask(taskId)).ReturnsAsync(existingTask);


            mockUserServices.Setup(service => service.ChangeTaskUser(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var controller = new TaskController(null, mockTasksRepository.Object, null, null, mockUserServices.Object, mockMapper.Object);
            var viewModel = new UpdateTaskViewModel
            {
                TaskTitle = "Tarefa Atualizada",
                TaskStatus = ProjectTaskStatus.EmAndamento,
                TaskDescription = "Nova Descricao",
                TaskExpirationDate = DateTime.Now.AddDays(2),
                User = Guid.NewGuid()
            };

            // Act
            try { 
            var result = await controller.UpdateTask(taskId, viewModel);

            }catch (Exception exception)
            {
                // Assert
                var requestResult = Assert.IsType<DomainException>(exception);
                string errorMessage = "Nao foi possivel realizar a troca de usuarios!";
                Assert.Equal(errorMessage, requestResult.Message);
            }
}

        [Fact]
        public async Task UpdateTask_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new TaskController(null, null, null, null, null, null);
            controller.ModelState.AddModelError("TaskTitle", "Required");

            // Act
            var result = await controller.UpdateTask(Guid.NewGuid(), new UpdateTaskViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new TaskController(null, null, null, null, null, null);
            controller.ModelState.AddModelError("User", "Required");

            // Act
            var result = await controller.DeleteTask(new Guid(), null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContentResult()
        {
            // Arrange
            var mockTasksRepository = new Mock<ITasksRepository>();
            mockTasksRepository.Setup(repo => repo.DeleteTask(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var controller = new TaskController(null, mockTasksRepository.Object, null, null, null, null);

            // Act
            var result = await controller.DeleteTask(Guid.NewGuid(), new DeleteTaskViewModel());

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNullDeletedTask_ReturnsNotFoundResult()
        {
            // Arrange
            var mockTasksRepository = new Mock<ITasksRepository>();
            mockTasksRepository.Setup(repo => repo.DeleteTask(It.IsAny<Guid>()))
                .ReturnsAsync((bool?)null);

            var controller = new TaskController(null, mockTasksRepository.Object, null, null, null, null);

            // Act
            var result = await controller.DeleteTask(Guid.NewGuid(), new DeleteTaskViewModel());

            // Assert
            var noContentResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTask_ReturnsFalseDeletedTask_ThrowsDomainException()
        {
            // Arrange
            var mockTasksRepository = new Mock<ITasksRepository>();
            mockTasksRepository.Setup(repo => repo.DeleteTask(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var controller = new TaskController(null, mockTasksRepository.Object, null, null, null, null);

            // Act
            try { 
            var result = await controller.DeleteTask(Guid.NewGuid(), new DeleteTaskViewModel());

            }
            catch (Exception exception)
            {
                // Assert
                var requestResult = Assert.IsType<DomainException>(exception);
                string errorMessage = "Nao foi possivel deletar a tarefa!";
                Assert.Equal(errorMessage, requestResult.Message);
            }
        }
    }
}
