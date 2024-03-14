using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor_Projetos_Tarefas.Api.ViewModels
{
    public class CreateTaskViewModel
    {
        public ProjectTaskStatus TaskStatus { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskExpirationDate { get; set; }
        public Guid User { get; set; }
        public string? Comment { get; set; }
    }
}
