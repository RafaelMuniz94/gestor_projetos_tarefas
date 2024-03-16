using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Gestor_Projetos_Tarefas.Api.ViewModels.Request
{
    [ExcludeFromCodeCoverage]
    public class CreateProjectViewModel
    {
        [Required(ErrorMessage = "O campo nome deve ser fornecido!")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "O campo descricao do projeto deve ser fornecido!")]
        public string ProjectDescription { get; set; }
    }
}
