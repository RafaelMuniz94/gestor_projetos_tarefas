using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.Models
{
    internal class TaskUpdateHistory
    {
        public TaskUpdateHistory()
        {
            
        }

        public Guid Id { get; set; }
        public DateTime ModificationTime { get; }

        public Guid User { get; set; }
        public String Change { get; set; }

        public Guid Task { get; set; }

        public string? Comment { get; set; }

    }
}
