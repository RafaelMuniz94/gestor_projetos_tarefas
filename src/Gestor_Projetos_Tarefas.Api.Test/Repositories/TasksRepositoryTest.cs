using Xunit;
using Gestor_Projetos_Tarefas.Database.Repositories;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Gestor_Projetos_Tarefas.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor_Projetos_Tarefas.Database.Context;
using Microsoft.Extensions.Configuration;

namespace Gestor_Projetos_Tarefas.Tests.Database.Repositories
{
    public class TasksRepositoryTest
    {

        private readonly DataBaseContext context;
        private readonly ProjectsRepository repository;

        public TasksRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "LocalTestDatabase")
                .Options;

            var configuration = new ConfigurationBuilder()
                .Build();

            context = new DataBaseContext(configuration, options);
            repository = new ProjectsRepository(context);
        }


        [Fact]
        public async Task AddTask_ShouldAddTaskToDatabase()
        {

            // Arrange
            var repository = new TasksRepository(context);
            var task = new ProjectTask
            {
                ID = Guid.NewGuid(),
                Title = "Tarefa Teste",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30)
                ,
                Project = Guid.NewGuid(),
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            };

            // Act
            var result = await repository.AddTask(task);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(task, result);

        }

        [Fact]
        public async Task DeleteTask_ShouldDeleteTaskFromDatabase()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskID = Guid.NewGuid();
            var task = new ProjectTask
            {
                ID = taskID,
                Title = "Tarefa Teste",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30)
                ,
                Project = Guid.NewGuid(),
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            };

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.DeleteTask(taskID);

            // Assert
            Assert.True(result);
            Assert.Empty(context.Tasks.Where(task => task.ID == taskID).Select(field => field.ID));

        }

        [Fact]
        public async Task ReturnTask_ShouldReturnTaskFromDatabase()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskID = Guid.NewGuid();
            var task = new ProjectTask
            {
                ID = taskID,
                Title = "Tarefa Teste",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30)
                ,
                Project = Guid.NewGuid(),
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            };

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ReturnTask(taskID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskID, result.ID);

        }

        [Fact]
        public async Task ReturnTaskListByProject_ShouldReturnTasksFromDatabase()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var projectID = Guid.NewGuid();
            var tasksToAdd = new List<ProjectTask>
             {
                new ProjectTask  {
                ID = Guid.NewGuid(),
                Title = "Tarefa 1",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30),
                Project =projectID,
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            },

               new ProjectTask  {
                ID = Guid.NewGuid(),
                Title = "Tarefa 2",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30),
                Project =projectID,
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            },

               new ProjectTask  {
                ID = Guid.NewGuid(),
                Title = "Tarefa 3",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30),
                Project =Guid.NewGuid(),
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            }
            };
            await context.Tasks.AddRangeAsync(tasksToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ReturnTasktListByProject(projectID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, task => Assert.Equal(projectID, task.Project));

        }

        [Fact]
        public async Task ReturnTaskListByUser_ShouldReturnTasksFromDatabase()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var userID = Guid.NewGuid();
            var tasksToAdd = new List<ProjectTask>
             {
                new ProjectTask  {
                ID = Guid.NewGuid(),
                Title = "Tarefa 1",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30),
                Project =Guid.NewGuid(),
                User = userID,
                Priority = TaskPriority.Baixa
            },

               new ProjectTask  {
                ID = Guid.NewGuid(),
                Title = "Tarefa 2",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30),
                Project =Guid.NewGuid(),
                User = userID,
                Priority = TaskPriority.Baixa
            },

               new ProjectTask  {
                ID = Guid.NewGuid(),
                Title = "Tarefa 3",
                Status = ProjectTaskStatus.Pendente,
                Description = "Teste",
                ExpirationDate = DateTime.Now.AddDays(30),
                Project =Guid.NewGuid(),
                User = Guid.NewGuid(),
                Priority = TaskPriority.Baixa
            }
            };
            await context.Tasks.AddRangeAsync(tasksToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ReturnTasktListByUser(userID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, task => Assert.Equal(userID, task.User));

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskInDatabase()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = "Tarefa Inicial",
                Status = ProjectTaskStatus.Pendente,
                Description = "Descricao",
                ExpirationDate = DateTime.Now.AddDays(7),
                Project = Guid.NewGuid(),
                User = Guid.NewGuid()
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
                Title = "Tarefa Atualizada",
                Status = ProjectTaskStatus.EmAndamento,
                Description = "Descricao 2",
                ExpirationDate = DateTime.Now.AddDays(10),
                Project = Guid.NewGuid(),
                User = Guid.NewGuid()
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.Equal("Tarefa Atualizada", result.Title);
            Assert.Equal(ProjectTaskStatus.EmAndamento, result.Status);
            Assert.Equal("Descricao 2", result.Description);
            Assert.Equal(DateTime.Now.AddDays(10).Date, result.ExpirationDate.Date);
            Assert.Equal(initialTask.Project, result.Project);
            Assert.Equal(initialTask.User, result.User);

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskOnlyTitleChange()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            string initialTitle = "Tarefa Inicial";
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = initialTitle,
                Status = ProjectTaskStatus.Pendente,
                Description = "Descricao",
                ExpirationDate = DateTime.Now.AddDays(7),
                Project = Guid.NewGuid(),
                User = Guid.NewGuid()
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
                Title = "Tarefa Atualizada"
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.NotEqual(initialTitle, result.Title);
            Assert.Equal(initialTask.Status, result.Status);
            Assert.Equal(initialTask.Description, result.Description);
            Assert.Equal(initialTask.ExpirationDate.Date, result.ExpirationDate.Date);
            Assert.Equal(initialTask.Project, result.Project);
            Assert.Equal(initialTask.User, result.User);

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskOnlyStatusChange()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            ProjectTaskStatus initialStatus = ProjectTaskStatus.Pendente;
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = "Tarefa Inicial",
                Status = initialStatus,
                Description = "Descricao",
                ExpirationDate = DateTime.Now.AddDays(7),
                Project = Guid.NewGuid(),
                User = Guid.NewGuid()
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
                Status = ProjectTaskStatus.Concluida,
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.Equal(initialTask.Title, result.Title);
            Assert.NotEqual(initialStatus, result.Status);
            Assert.Equal(initialTask.Description, result.Description);
            Assert.Equal(initialTask.ExpirationDate.Date, result.ExpirationDate.Date);
            Assert.Equal(initialTask.Project, result.Project);
            Assert.Equal(initialTask.User, result.User);

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskOnlyDescriptionChange()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            string initialDescription = "Descricao Inicial";
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = "Tarefa Inicial",
                Status = ProjectTaskStatus.Pendente,
                Description = initialDescription,
                ExpirationDate = DateTime.Now.AddDays(7),
                Project = Guid.NewGuid(),
                User = Guid.NewGuid()
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
               Description = "Descricao Atualizada"
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.Equal(initialTask.Title, result.Title);
            Assert.Equal(initialTask.Status, result.Status);
            Assert.NotEqual(initialDescription, result.Description);
            Assert.Equal(initialTask.ExpirationDate.Date, result.ExpirationDate.Date);
            Assert.Equal(initialTask.Project, result.Project);
            Assert.Equal(initialTask.User, result.User);

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskOnlyExpirationDateChange()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            DateTime initialExpirationDate = DateTime.Now;
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = "Tarefa Inicial",
                Status = ProjectTaskStatus.Pendente,
                Description = "Descricao Inicial",
                ExpirationDate = initialExpirationDate,
                Project = Guid.NewGuid(),
                User = Guid.NewGuid()
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
                ExpirationDate = DateTime.Now.AddDays(10)
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.Equal(initialTask.Title, result.Title);
            Assert.Equal(initialTask.Status, result.Status);
            Assert.Equal(initialTask.Description, result.Description);
            Assert.NotEqual(initialExpirationDate, result.ExpirationDate.Date);
            Assert.Equal(initialTask.Project, result.Project);
            Assert.Equal(initialTask.User, result.User);

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskOnlyProjectChange()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            Guid initialProject = Guid.NewGuid();
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = "Tarefa Inicial",
                Status = ProjectTaskStatus.Pendente,
                Description = "Descricao Inicial",
                ExpirationDate = DateTime.Now.AddDays(7),
                Project = initialProject,
                User = Guid.NewGuid()
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
                Project = Guid.NewGuid()
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.Equal(initialTask.Title, result.Title);
            Assert.Equal(initialTask.Status, result.Status);
            Assert.Equal(initialTask.Description, result.Description);
            Assert.Equal(initialTask.ExpirationDate.Date, result.ExpirationDate.Date);
            Assert.NotEqual(initialProject, result.Project);
            Assert.Equal(initialTask.User, result.User);

        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTaskOnlyUserChange()
        {
            // Arrange
            var repository = new TasksRepository(context);
            var taskId = Guid.NewGuid();
            Guid initialUser = Guid.NewGuid();
            var initialTask = new ProjectTask
            {
                ID = taskId,
                Title = "Tarefa Inicial",
                Status = ProjectTaskStatus.Pendente,
                Description = "Descricao Inicial",
                ExpirationDate = DateTime.Now.AddDays(7),
                Project = Guid.NewGuid(),
                User = initialUser
            };
            await context.Tasks.AddAsync(initialTask);
            await context.SaveChangesAsync();

            var updatedTaskDto = new UpdateProjectTaskDTO
            {
                ID = taskId,
                User = Guid.NewGuid()
            };

            // Act
            var result = await repository.UpdateTask(updatedTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.ID);
            Assert.Equal(initialTask.Title, result.Title);
            Assert.Equal(initialTask.Status, result.Status);
            Assert.Equal(initialTask.Description, result.Description);
            Assert.Equal(initialTask.ExpirationDate.Date, result.ExpirationDate.Date);
            Assert.Equal(initialTask.Project, result.Project);
            Assert.NotEqual(initialUser, result.User);

        }

    }
}
