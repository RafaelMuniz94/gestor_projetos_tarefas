using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Gestor_Projetos_Tarefas.Api.Controllers;
using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Api.ViewModels.Request;
using Gestor_Projetos_Tarefas.Api.ViewModels.Return;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;

namespace Gestor_Projetos_Tarefas.Tests.Controllers
{
    public class ProjectControllerTests
    {
        [Fact]
        public async Task ReturnProjects_ReturnsOkResult()
        {
            // Arrange
            var mockProjectsRepository = new Mock<IProjectsRepository>();
            mockProjectsRepository.Setup(repository => repository.ReturnProjectList(It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<Project>());

            var controller = new ProjectController(mockProjectsRepository.Object, null, null);

            // Act
            var result = await controller.ReturnProjects();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var projects = Assert.IsAssignableFrom<List<Project>>(okResult.Value);
            Assert.Empty(projects);
        }

        [Fact]
        public async Task ReturnProjectsByUser_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserServices>();
            mockUserService.Setup(service => service.ReturnUser(userId))
                .ReturnsAsync(new User());

            var controller = new ProjectController(null, null, mockUserService.Object);

            // Act
            var result = await controller.ReturnProjectsByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var projects = Assert.IsAssignableFrom<List<Project>>(okResult.Value);
            Assert.Empty(projects);
        }

        [Fact]
        public async Task ReturnProjectsByUser_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserServices>();
            mockUserService.Setup(service => service.ReturnUser(userId))
                .ReturnsAsync((User)null);

            var controller = new ProjectController(null, null, mockUserService.Object);

            // Act
            var result = await controller.ReturnProjectsByUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }


        [Fact]
        public async Task ReturnProjectDetail_ReturnsOkResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var mockProjectRepository = new Mock<IProjectsRepository>();
            mockProjectRepository.Setup(repo => repo.ReturnProject(projectId))
                .ReturnsAsync(new Project());

            var mockTasksRepository = new Mock<ITasksRepository>();
            mockTasksRepository.Setup(repo => repo.ReturnTasktListByProject(projectId))
                .ReturnsAsync(new List<ProjectTask>());

            var controller = new ProjectController(mockProjectRepository.Object, mockTasksRepository.Object, null);

            // Act
            var result = await controller.ReturnProjectDetail(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var projectDetails = Assert.IsAssignableFrom<ProjectDetailsViewModel>(okResult.Value);
            Assert.NotNull(projectDetails);
            Assert.NotNull(projectDetails.project);
            Assert.NotNull(projectDetails.tasks);
            Assert.Empty(projectDetails.tasks);
        }

        [Fact]
        public async Task ReturnProjectDetail_ReturnsNotFoundResult()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var mockProjectRepository = new Mock<IProjectsRepository>();
            mockProjectRepository.Setup(repo => repo.ReturnProject(projectId))
                .ReturnsAsync((Project)null);


            var controller = new ProjectController(mockProjectRepository.Object, null, null);

            // Act
            var result = await controller.ReturnProjectDetail(projectId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task CreateProject_WithValidModel_ReturnsCreatedResult()
        {
            // Arrange
            var mockProjectRepository = new Mock<IProjectsRepository>();
            mockProjectRepository.Setup(repo => repo.AddProject(It.IsAny<Project>()))
                .ReturnsAsync(new Project());

            var controller = new ProjectController(mockProjectRepository.Object, null, null);
            var viewModel = new CreateProjectViewModel { ProjectName = "Projeto teste", ProjectDescription = "Descricao" };

            // Act
            var result = await controller.CreateProject(viewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateProject_WithNullProject_ReturnsBadRequestResult()
        {
            // Arrange
            var mockProjectRepository = new Mock<IProjectsRepository>();
            mockProjectRepository.Setup(repo => repo.AddProject(It.IsAny<Project>()))
                .ReturnsAsync((Project)null);

            var controller = new ProjectController(mockProjectRepository.Object, null, null);
            var viewModel = new CreateProjectViewModel { ProjectName = "Projeto teste", ProjectDescription = "Descricao" };

            // Act
            var result = await controller.CreateProject(viewModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateProject_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new ProjectController(null, null, null);
            controller.ModelState.AddModelError("ProjectName", "Required");

            // Act
            var result = await controller.CreateProject(new CreateProjectViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
