using System.ComponentModel.DataAnnotations;

namespace Gestor_Projetos_Tarefas.Api.ViewModels.Request
{
    public class DeleteTaskViewModel
    {
        [Required(ErrorMessage = "O ID do usuario deve ser fornecido!")]
        public Guid User { get; set; }

        public string? Comment { get; set; }
    }
}
