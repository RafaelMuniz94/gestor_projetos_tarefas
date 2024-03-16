using AutoMapper;
using Gestor_Projetos_Tarefas.Api.Mappers;
using Gestor_Projetos_Tarefas.Api.ViewModels.Request;
using Gestor_Projetos_Tarefas.Domain.DTOs;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using Xunit;

namespace Gestor_Projetos_Tarefas.Tests.Mappers
{
    public class TaskUpdateMapperTests
    {
        private readonly IMapper mapper;

        public TaskUpdateMapperTests()
        {
            var mapperConfig = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile<TaskUpdateMapper>();
            });
            mapper = mapperConfig.CreateMapper();
        }


        [Fact]
        public void TaskUpdateMapper_MapViewModelToDTO_IsValid()
        {
            // Arrange
            var viewModel = new UpdateTaskViewModel
            {
                TaskTitle = "Task Teste",
                TaskStatus = ProjectTaskStatus.EmAndamento,
                TaskExpirationDate = new DateTime(2024, 12, 31),
                User = Guid.NewGuid(),
                Project = Guid.NewGuid(),
                TaskDescription = "Test Description"
            };

            // Act
            var resultDto = mapper.Map<UpdateProjectTaskDTO>(viewModel);

            // Assert
            Assert.NotNull(resultDto);
            Assert.Equal(viewModel.TaskTitle, resultDto.Title);
            Assert.Equal(viewModel.TaskStatus, resultDto.Status);
            Assert.Equal(viewModel.TaskExpirationDate, resultDto.ExpirationDate);
            Assert.Equal(viewModel.User, resultDto.User);
            Assert.Equal(viewModel.Project, resultDto.Project);
            Assert.Equal(viewModel.TaskDescription, resultDto.Description);
        }
    }
}
