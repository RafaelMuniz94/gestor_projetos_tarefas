using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.DTOs
{
    public class UpdateProjectTaskDTO
    {
        public Guid ID { get; set; }
        public TasksStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public Guid? Project { get; set; }
        public Guid? User { get; set; }
    }
}
