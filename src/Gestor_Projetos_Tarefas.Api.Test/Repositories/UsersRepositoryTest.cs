using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Repositories;
using Gestor_Projetos_Tarefas.Domain.Models;
using Microsoft.Extensions.Configuration;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System.Xml.Linq;

namespace Gestor_Projetos_Tarefas.Tests.Database.Repositories
{
    public class UsersRepositoryTest
    {
        private readonly DataBaseContext context;
        private readonly UsersRepository repository;

        public UsersRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "LocalTestDatabase4")
                .Options;

            var configuration = new ConfigurationBuilder()
                .Build();

            context = new DataBaseContext(configuration, options);
            repository = new UsersRepository(context);
        }


        [Fact]
        public async Task AddProject_Should_AddProjectToUser()
        {
            // Arrange
            var userID = Guid.NewGuid();
            var user = new User { ID = userID, Email = "email@teste.com", Name = "User1", Role = Role.Analista, Projects = new List<Project>()};

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var projectID = Guid.NewGuid();
            var project = new Project { ID = projectID, Name = "Projeto Teste", Description = "Decricao" };

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.AddProject(user.ID, project);


            // Assert
            Assert.True(result);
            Assert.Contains(project, user.Projects);
        }

        [Fact]
        public async Task RemoveProjectFromUser_Should_RemoveProjectFromUser()
        {
            // Arrange
            var projectID = Guid.NewGuid();
            var project = new Project { ID = projectID, Name = "Projeto Teste", Description = "Decricao" };

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            var userID = Guid.NewGuid();
            var user = new User { ID = userID, Email = "email@teste.com", Name = "User1", Role = Role.Analista, Projects = new List<Project> { project } };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.RemoveProjectFromUser(user.ID, project.ID);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(project, user.Projects);

        }

        [Fact]
        public async Task ReturnUser_Should_ReturnUser()
        {
            // Arrange

            var userID = Guid.NewGuid();
            var user = new User { ID = userID, Email = "email@teste.com", Name = "User1", Role = Role.Analista, Projects = new List<Project>() };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ReturnUser(user.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.ID, result.ID);

        }
    }
}
