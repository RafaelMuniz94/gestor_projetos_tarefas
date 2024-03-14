using AutoMapper;
using Gestor_Projetos_Tarefas.Api.ViewModels.Request;
using Gestor_Projetos_Tarefas.Domain.DTOs;

namespace Gestor_Projetos_Tarefas.Api.Mappers
{
    public class TaskUpdateMapper : Profile
    {
        public TaskUpdateMapper()
        {
            CreateMap<UpdateTaskViewModel, UpdateProjectTaskDTO>()
                .ForMember(destination => destination.Title, member => member.MapFrom(source => source.TaskTitle))
                .ForMember(destination => destination.Status, member => member.MapFrom(source => source.TaskStatus))
                .ForMember(destination => destination.ExpirationDate, member => member.MapFrom(source => source.TaskExpirationDate))
                .ForMember(destination => destination.User, member => member.MapFrom(source => source.User))
                .ForMember(destination => destination.Project, member => member.MapFrom(source => source.Project))
                .ForMember(destination => destination.Description, member => member.MapFrom(source => source.TaskDescription));
        }
    }
}
