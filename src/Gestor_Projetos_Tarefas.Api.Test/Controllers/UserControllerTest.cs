using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor_Projetos_Tarefas.Api.Controllers;
using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Exceptions;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Gestor_Projetos_Tarefas.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task RemoveProject_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(projectId)).ReturnsAsync((Project)null);
            var tasksRepositoryMock = new Mock<ITasksRepository>();
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repo => repo.RemoveProjectFromUser(userId, projectId)).ReturnsAsync(false);
            var controller = new UserController(projectsRepositoryMock.Object, tasksRepositoryMock.Object, usersRepositoryMock.Object, null, null);

            // Act
            var result = await controller.RemoveProject(userId, projectId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Projeto não encontrado!", notFoundResult.Value);
        }

        [Fact]
        public async Task RemoveProject_OpenedTasksExist_ThrowsDomainException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            List<ProjectTask> tasks = new List<ProjectTask>
            {
                new ProjectTask { User = userId, Status = ProjectTaskStatus.Pendente }
            };
            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(projectId)).ReturnsAsync(new Project());
            var tasksRepositoryMock = new Mock<ITasksRepository>();
            tasksRepositoryMock.Setup(repo => repo.ReturnTasktListByProject(projectId)).ReturnsAsync(tasks);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var controller = new UserController(projectsRepositoryMock.Object, tasksRepositoryMock.Object, usersRepositoryMock.Object, null, null);

            try
            {
                var result = await controller.RemoveProject(userId, projectId);
            }
            catch (Exception exception)
            {
                // Assert
                var requestResult = Assert.IsType<DomainException>(exception);
                string errorMessage = "Nao sera possivel remover a tarefa, existem tarefas nao concluidas para o usuario, favor finalizar!";
                Assert.Equal(errorMessage, requestResult.Message);
            }
        }

        [Fact]
        public async Task RemoveProject_ProjectRemovalReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repository => repository.ReturnProject(projectId)).ReturnsAsync(new Project());
            
            var taskRepositoryMock = new Mock<ITasksRepository>();
            taskRepositoryMock.Setup(repository => repository.ReturnTasktListByProject(It.IsAny<Guid>())).ReturnsAsync(new List<ProjectTask>());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repository => repository.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((bool?)null);

            var controller = new UserController(projectsRepositoryMock.Object, taskRepositoryMock.Object, usersRepositoryMock.Object, null, null);

            // Act
            var result = await controller.RemoveProject(userId, projectId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("O projeto ou usuario nao foram encontrados!", notFoundResult.Value);

        }

        [Fact]
        public async Task RemoveProject_ProjectRemovalReturnsFalse_ThrowsDomainException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repository => repository.ReturnProject(projectId)).ReturnsAsync(new Project());

            var taskRepositoryMock = new Mock<ITasksRepository>();
            taskRepositoryMock.Setup(repository => repository.ReturnTasktListByProject(It.IsAny<Guid>())).ReturnsAsync(new List<ProjectTask>());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repository => repository.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = new UserController(projectsRepositoryMock.Object, taskRepositoryMock.Object, usersRepositoryMock.Object, null, null);


            try
            {
                // Act
                var result = await controller.RemoveProject(userId, projectId);
            }
            catch (Exception exception)
            {
                // Assert
                var requestResult = Assert.IsType<DomainException>(exception);
                string errorMessage = "Nao foi possivel remover o projeto!";
                Assert.Equal(errorMessage, requestResult.Message);
            }
        }

        [Fact]
        public async Task RemoveProject_ProjectRemovalReturnsTrue_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repository => repository.ReturnProject(projectId)).ReturnsAsync(new Project());

            var taskRepositoryMock = new Mock<ITasksRepository>();
            taskRepositoryMock.Setup(repository => repository.ReturnTasktListByProject(It.IsAny<Guid>())).ReturnsAsync(new List<ProjectTask>());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repository => repository.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var controller = new UserController(projectsRepositoryMock.Object, taskRepositoryMock.Object, usersRepositoryMock.Object, null, null);

            // Act
            var result = await controller.RemoveProject(userId, projectId);
            // Assert
            var notContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, notContentResult.StatusCode);

        }

        [Fact]
        public async Task RemoveProject_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(projectId)).ReturnsAsync((Project)null);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            var controller = new UserController(projectsRepositoryMock.Object, null, usersRepositoryMock.Object, null, null);

            // Act
            var result = await controller.RemoveProject(userId, projectId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Projeto não encontrado!", notFoundResult.Value);
        }

        [Fact]
        public async Task GeneratePerformanceReport_RequesterNotManager_ThrowsDomainException()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var requesterId = Guid.NewGuid();
            var requester = new User { Role = Role.Analista }; // Not a manager
            var usersServicesMock = new Mock<IUserServices>();
            usersServicesMock.Setup(service => service.ReturnUser(requesterId)).ReturnsAsync(requester);
            var controller = new UserController(null, null, null, null, usersServicesMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() => controller.GeneratePerformanceReport(targetUserId, requesterId));
        }

        [Fact]
        public async Task GeneratePerformanceReport_NoTasksForUser_ReturnsBadRequest()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var requesterId = Guid.NewGuid();
            var requester = new User { Role = Role.Gerente };

            var usersServicesMock = new Mock<IUserServices>();
            usersServicesMock.Setup(service => service.ReturnUser(requesterId)).ReturnsAsync(requester);

            var tasksRepositoryMock = new Mock<ITasksRepository>();
            tasksRepositoryMock.Setup(repository => repository.ReturnTasktListByUser(It.IsAny<Guid>())).ReturnsAsync(new List<ProjectTask>());
            var controller = new UserController(null, tasksRepositoryMock.Object, null, null, usersServicesMock.Object);

            // Act
            var result = await controller.GeneratePerformanceReport(targetUserId, requesterId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"O Usuario {targetUserId} nao possui tarefas", badRequestResult.Value);
        }

        [Fact]
        public async Task GeneratePerformanceReport_NoTasksCompletedInLast30Days_ReturnsOk()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var requesterId = Guid.NewGuid();
            var requester = new User { Role = Role.Gerente }; 

            var usersServicesMock = new Mock<IUserServices>();
            usersServicesMock.Setup(service => service.ReturnUser(requesterId)).ReturnsAsync(requester);

            List<ProjectTask> tasks = new List<ProjectTask>
            {
                new ProjectTask { User = targetUserId, Status = ProjectTaskStatus.Concluida, ExpirationDate = DateTime.Now.AddDays(30) }
            };

            var tasksRepositoryMock = new Mock<ITasksRepository>();

            tasksRepositoryMock.Setup(repo => repo.ReturnTasktListByUser(targetUserId)).ReturnsAsync(tasks);

            var historyRepositoryMock = new Mock<IHistoryUpdateProjectRepository>();
            historyRepositoryMock.Setup(repository => repository.ReturnHistoryByUser(It.IsAny<Guid>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Guid>());

            var controller = new UserController(null, tasksRepositoryMock.Object, null, historyRepositoryMock.Object, usersServicesMock.Object);

            // Act
            var result = await controller.GeneratePerformanceReport(targetUserId, requesterId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"O Usuario {targetUserId} nao finalizou tarefas nos ultimos 30 dias.", okResult.Value);
        }

        [Fact]
        public async Task GeneratePerformanceReport_TasksCompletedInLast30Days_ReturnsOk()
        {
            // Arrange
            var targetUserId = Guid.NewGuid();
            var requesterId = Guid.NewGuid();
            var requester = new User { Role = Role.Gerente };

            var usersServicesMock = new Mock<IUserServices>();
            usersServicesMock.Setup(service => service.ReturnUser(requesterId)).ReturnsAsync(requester);

            List<ProjectTask> tasks = new List<ProjectTask>
            {
                new ProjectTask { User = targetUserId, Status = ProjectTaskStatus.Concluida, ExpirationDate = DateTime.Now.AddDays(60) }
            };

            List<Guid> completeTasks = new List<Guid> {
                new Guid()
            };

            var tasksRepositoryMock = new Mock<ITasksRepository>();
            tasksRepositoryMock.Setup(repo => repo.ReturnTasktListByUser(targetUserId)).ReturnsAsync(tasks);
            var historyRepositoryMock = new Mock<IHistoryUpdateProjectRepository>();
            historyRepositoryMock.Setup(repository => repository.ReturnHistoryByUser(It.IsAny<Guid>(), It.IsAny<DateTime>())).ReturnsAsync(completeTasks);

            var controller = new UserController(null, tasksRepositoryMock.Object, null, historyRepositoryMock.Object, usersServicesMock.Object);

            // Act
            var result = await controller.GeneratePerformanceReport(targetUserId, requesterId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains($"O usuario {targetUserId} realizou 1 das 1, totalizando um total de 100% de finalizacoes!", (string)okResult.Value);
        }
    }
}
