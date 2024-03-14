using Gestor_Projetos_Tarefas.Domain.Models;

namespace Gestor_Projetos_Tarefas.Api.ViewModels.Return
{
    public class ProjectDetailsViewModel
    {
       public Project project { get; set; }
       public List<ProjectTask> tasks { get; set; }
    }
}
