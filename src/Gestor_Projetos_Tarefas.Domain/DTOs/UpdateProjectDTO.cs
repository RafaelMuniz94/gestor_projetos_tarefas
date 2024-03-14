using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.DTOs
{
    public class UpdateProjectDTO
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
