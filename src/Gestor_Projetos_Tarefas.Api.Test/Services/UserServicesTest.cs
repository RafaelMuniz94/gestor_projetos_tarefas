using Xunit;
using Moq;
using Gestor_Projetos_Tarefas.Api.Services;
using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Test.Services
{
    public class UserServicesTests
    {
        [Fact]
        public async Task ChangeTaskUser_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            var projectID = Guid.NewGuid();
            var oldUserID = Guid.NewGuid();
            var newUserID = Guid.NewGuid();

            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(It.IsAny<Guid>())).ReturnsAsync((Project)null);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            var tasksRepositoryMock = new Mock<ITasksRepository>();

            var userServices = new UserServices(projectsRepositoryMock.Object, usersRepositoryMock.Object, tasksRepositoryMock.Object);

            // Act
            var result = await userServices.ChangeTaskUser(oldUserID, newUserID, projectID);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task ChangeTaskUser_RemoveProjectFromUser_ReturnsNull()
        {
            // Arrange
            var projectID = Guid.NewGuid();
            var oldUserID = Guid.NewGuid();
            var newUserID = Guid.NewGuid();

            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(It.IsAny<Guid>())).ReturnsAsync(new Project());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repo => repo.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((bool?)null);

           
            var tasksRepositoryMock = new Mock<ITasksRepository>();

            var userServices = new UserServices(projectsRepositoryMock.Object, usersRepositoryMock.Object, tasksRepositoryMock.Object);

            // Act
            var result = await userServices.ChangeTaskUser(oldUserID, newUserID, projectID);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ChangeTaskUser_AddProject_ReturnsFalse()
        {
            // Arrange
            var projectID = Guid.NewGuid();
            var oldUserID = Guid.NewGuid();
            var newUserID = Guid.NewGuid();

           

            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(It.IsAny<Guid>())).ReturnsAsync(new Project());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repo => repo.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            usersRepositoryMock.Setup(repo => repo.AddProject(It.IsAny<Guid>(), It.IsAny<Project>())).ReturnsAsync(false);


            var tasksRepositoryMock = new Mock<ITasksRepository>();

            var userServices = new UserServices(projectsRepositoryMock.Object, usersRepositoryMock.Object, tasksRepositoryMock.Object);

            // Act
            var result = await userServices.ChangeTaskUser(oldUserID, newUserID, projectID);
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ChangeTaskUser_AddProject_ReturnsTrue_RemoveProjectFromUser_ReturnsFalse()
        {
            // Arrange
            var projectID = Guid.NewGuid();
            var oldUserID = Guid.NewGuid();
            var newUserID = Guid.NewGuid();



            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(It.IsAny<Guid>())).ReturnsAsync(new Project());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repo => repo.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);
            usersRepositoryMock.Setup(repo => repo.AddProject(It.IsAny<Guid>(), It.IsAny<Project>())).ReturnsAsync(true);


            var tasksRepositoryMock = new Mock<ITasksRepository>();

            var userServices = new UserServices(projectsRepositoryMock.Object, usersRepositoryMock.Object, tasksRepositoryMock.Object);

            // Act
            var result = await userServices.ChangeTaskUser(oldUserID, newUserID, projectID);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ChangeTaskUser_ReturnsTrue()
        {
            // Arrange
            var projectID = Guid.NewGuid();
            var oldUserID = Guid.NewGuid();
            var newUserID = Guid.NewGuid();



            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProject(It.IsAny<Guid>())).ReturnsAsync(new Project());

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repo => repo.RemoveProjectFromUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            usersRepositoryMock.Setup(repo => repo.AddProject(It.IsAny<Guid>(), It.IsAny<Project>())).ReturnsAsync(true);


            var tasksRepositoryMock = new Mock<ITasksRepository>();

            var userServices = new UserServices(projectsRepositoryMock.Object, usersRepositoryMock.Object, tasksRepositoryMock.Object);

            // Act
            var result = await userServices.ChangeTaskUser(oldUserID, newUserID, projectID);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task ReturnUser_ValidUserID_ReturnsUserWithProjects()
        {
            // Arrange
            var userID = Guid.NewGuid();
            var user = new User { ID = userID };
            var tasks = new List<ProjectTask>
            {
                new ProjectTask { ID = Guid.NewGuid(), User = userID, Project = Guid.NewGuid(), Status = ProjectTaskStatus.Concluida },
                new ProjectTask { ID = Guid.NewGuid(), User = userID, Project = Guid.NewGuid(), Status = ProjectTaskStatus.EmAndamento },
                new ProjectTask { ID = Guid.NewGuid(), User = userID, Project = Guid.NewGuid(), Status = ProjectTaskStatus.Pendente }
            };
            var projectIDs = tasks.Where(t => t.Status != ProjectTaskStatus.Concluida).Select(t => t.Project).ToList();

            var projects = projectIDs.Select(projectID => new Project { ID = projectID }).ToList();

            var projectsRepositoryMock = new Mock<IProjectsRepository>();
            projectsRepositoryMock.Setup(repo => repo.ReturnProjectList(It.IsAny<List<Guid>>())).ReturnsAsync(projects);

            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(repo => repo.ReturnUser(userID)).ReturnsAsync(user);

            var tasksRepositoryMock = new Mock<ITasksRepository>();
            tasksRepositoryMock.Setup(repo => repo.ReturnTasktListByUser(userID)).ReturnsAsync(tasks);

            var userServices = new UserServices(projectsRepositoryMock.Object, usersRepositoryMock.Object, tasksRepositoryMock.Object);

            // Act
            var result = await userServices.ReturnUser(userID);

            // Assert
            Assert.Equal(user, result);
            Assert.Equal(projects, result.Projects);
        }
    }
}
