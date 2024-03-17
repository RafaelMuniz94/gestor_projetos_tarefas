using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Repositories;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Gestor_Projetos_Tarefas.Tests.Database.Repositories
{
    public class HistoryUpdateProjectRepositoryTest
    {
        private readonly DataBaseContext context;
        private readonly HistoryUpdateProjectRepository repository;

        public HistoryUpdateProjectRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName: "LocalTestDatabase")
                .Options;

            var configuration = new ConfigurationBuilder()
                .Build();

            context = new DataBaseContext(configuration, options);
            repository = new HistoryUpdateProjectRepository(context);
        }

        [Fact]
        public async Task AddHistoryRecord_ValidRecord_ReturnsTrue()
        {
            // Arrange
            var record = new TaskUpdateHistory
            {
                ID = Guid.NewGuid(),
                ModificationTime = DateTime.Now,
                User = Guid.NewGuid(),
                Change = "Criacao",
                Task = Guid.NewGuid(),
                Comment = "Teste criacao de historico"
            };

            // Act
            var result = await repository.AddHistoryRecord(record);

            // Assert
            Assert.True(result);

        }

        [Fact]
        public async Task ReturnHistoryByUser_ValidInput_ReturnsListOfGuids()
        {
            // Arrange
            var dateLimit = DateTime.Now.AddDays(-30);
            DateTime date;
            var tenDaysAgo = DateTime.Now.AddDays(-10);
            var thirtyOneDaysAgo = DateTime.Now.AddDays(-31);

            var projectID = Guid.NewGuid();
            var project = new Project { ID = projectID, Name = "Projeto Teste", Description = "Decricao" };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var userID = Guid.NewGuid();
            var user = new User { ID = userID, Email="email@teste.com", Name= "User1", Role = Role.Analista, Projects = new List<Project>()};
            context.Users.Add(user);
            await context.SaveChangesAsync();

            List<Guid> mustContainIDS = new List<Guid>();
            List<Guid> mustNotContainIDS = new List<Guid>();



            for (int index =0; index <6; index++)
            {
                var taskID = Guid.NewGuid();


                if(index < 3)
                {
                    date = tenDaysAgo;
                    mustContainIDS.Add(taskID);
                }
                else
                {
                    date = thirtyOneDaysAgo;
                    mustNotContainIDS.Add(taskID);
                }


                var task = new ProjectTask
                {
                    ID = taskID,
                    Description = "Uma tarefa",
                    ExpirationDate = date.AddDays(30),
                    Priority = TaskPriority.Baixa,
                    Project = projectID,
                    Status = ProjectTaskStatus.Concluida,
                    Title = "Nova Tarefa",
                    User = userID
                };
                context.Tasks.Add(task);
                await context.SaveChangesAsync();

                var taskUpdateHistory = new TaskUpdateHistory
                {
                    ID = Guid.NewGuid(),
                    ModificationTime = date,
                    User =userID,
                    Change = "Criacao",
                    Task = taskID,
                    Comment = "Tarefa finalizada"
                };

                context.History.Add(taskUpdateHistory);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await repository.ReturnHistoryByUser(userID, dateLimit);

            // Assert
            Assert.IsType<List<Guid>>(result);
            Assert.True(result.Any(id => mustContainIDS.Any(contains => contains == id)));
            Assert.False(result.Any(id => mustNotContainIDS.Any(notContains => notContains == id)));

        }
    }
}
