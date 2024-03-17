using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Repositories;
using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Gestor_Projetos_Tarefas.Tests.Database.Repositories
{
    public class ProjectsRepositoryTest
    {
        private readonly DataBaseContext context;
        private readonly ProjectsRepository repository;

        public ProjectsRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "LocalTestDatabase")
                .Options;

            var configuration = new ConfigurationBuilder()
                .Build();

            context = new DataBaseContext(configuration,options);
            repository = new ProjectsRepository(context);
        }


        [Fact]
        public async Task AddProject_ValidProject_ReturnsProject()
        {
            // Arrange
            var project = new Project { ID = Guid.NewGuid(), Name = "Projeto Teste", Description = "Decricao" };

            // Act
            var result = await repository.AddProject(project);


            // Assert
           Assert.NotNull(result);
           Assert.Equal(project.ID, result.ID);
        }

        [Fact]
        public async Task DeleteProject_ExistingProjectID_ReturnsTrue()
        {
            // Arrange
            var project = new Project { ID = Guid.NewGuid(), Name = "Projeto Teste", Description = "Decricao" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteProject(project.ID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProject_NonExistingProjectID_ReturnsNull()
        {
            // Arrange
            var nonExistingProjectID = Guid.NewGuid();

            // Act
            var result = await repository.DeleteProject(nonExistingProjectID);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnProject_ExistingProjectID_ReturnsProject()
        {
            // Arrange
            var project = new Project { ID = Guid.NewGuid(), Name = "Projeto Teste", Description = "Decricao" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ReturnProject(project.ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.ID, result.ID);
        }

        [Fact]
        public async Task ReturnProject_NonExistingProjectID_ReturnsNull()
        {
            // Arrange
            var nonExistingProjectID = Guid.NewGuid();

            // Act
            var result = await repository.ReturnProject(nonExistingProjectID);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnProjectList_WithProjectIDs_ReturnsProjects()
        {
            // Arrange
            var projectIDs = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            foreach (Guid ID in projectIDs)
            {
                context.Projects.Add(new Project { ID = ID ,Name= $"Projeto - {ID}", Description=$"{ID}"});
            }
            await context.SaveChangesAsync();
            // Act
            var result = await repository.ReturnProjectList(projectIDs);

            // Assert
            Assert.Equal(projectIDs.Count, result.Count);
            Assert.Equal(projectIDs, result.Select(p => p.ID).ToList());
        }

        [Fact]
        public async Task ReturnProjectList_WithoutProjectIDs_ReturnsAllProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { ID = Guid.NewGuid(),Name= "Projeto - 1", Description=$"Projeto novo 1 " },
                new Project { ID = Guid.NewGuid(),Name= "Projeto - 2", Description=$"Projeto novo 2 " }
            };

            context.Projects.RemoveRange(context.Projects);
            await context.SaveChangesAsync();


            foreach (var project in projects)
            {
                context.Projects.Add(project);
            }
            
            await context.SaveChangesAsync();
            
            // Act
            var result = await repository.ReturnProjectList(null);

            // Assert
            Assert.Equal(projects.Count, result.Count);
        }

        [Fact]
        public async Task UpdateProject_ExistingProjectDTO_ReturnsUpdatedProject()
        {
            // Arrange
            var project = new Project { ID = Guid.NewGuid(), Name = "Projeto Teste", Description = "Decricao" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var updatedProjectDTO = new UpdateProjectDTO
            {
                ID = project.ID,
                Name = "Updated Projeto Teste",
                Description = "Updated Decricao"
            };

            // Act
            var result = await repository.UpdateProject(updatedProjectDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProjectDTO.Name, result.Name);
            Assert.Equal(updatedProjectDTO.Description, result.Description);
        }

        [Fact]
        public async Task UpdateProject_NonExistingProjectDTO_ReturnsNull()
        {
            // Arrange
            var nonExistingProjectDTO = new UpdateProjectDTO
            {
                ID = Guid.NewGuid(),
                Name = "Updated Projeto Teste",
                Description = "Updated Decricao"
            };

            // Act
            var result = await repository.UpdateProject(nonExistingProjectDTO);

            // Assert
            Assert.Null(result);
        }
    }
}
