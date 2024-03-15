using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor_Projetos_Tarefas.Api.ViewModels.Request
{
    public class CreateTaskViewModel
    {
        [Required(ErrorMessage = "O campo status deve ser fornecido!")]
        public ProjectTaskStatus TaskStatus { get; set; }

        [Required(ErrorMessage = "O campo prioridade deve ser fornecido!")]
        public TaskPriority TaskPriority { get; set; }

        [Required(ErrorMessage = "O campo nome titulo ser fornecido!")]
        public string TaskTitle { get; set; }

        [Required(ErrorMessage = "O campo descricao deve ser fornecido!")]
        public string TaskDescription { get; set; }

        [Required(ErrorMessage = "O campo data de validade deve ser fornecido!")]
        [DataType(DataType.Date)]
        public DateTime TaskExpirationDate { get; set; }

        [Required(ErrorMessage = "O ID do usuario deve ser fornecido!")]
        public Guid User { get; set; }

        public string? Comment { get; set; }
    }
}
