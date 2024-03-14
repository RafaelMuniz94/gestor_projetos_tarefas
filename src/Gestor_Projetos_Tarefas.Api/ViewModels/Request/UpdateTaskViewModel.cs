using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Gestor_Projetos_Tarefas.Api.ViewModels.Request
{
    public class UpdateTaskViewModel
    {

        public ProjectTaskStatus? TaskStatus { get; set; }


        public string? TaskTitle { get; set; }


        public string? TaskDescription { get; set; }


        [DataType(DataType.Date)]
        public DateTime? TaskExpirationDate { get; set; }


        public Guid? User { get; set; }
        public Guid? Project { get; set; }
        public string? Comment { get; set; }
    }
}
