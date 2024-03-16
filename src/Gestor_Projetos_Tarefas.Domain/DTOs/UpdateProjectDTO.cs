using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UpdateProjectDTO
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
