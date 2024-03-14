using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.Models
{
    [Table("taskUpdateHistory")]
    public class TaskUpdateHistory
    {
        public TaskUpdateHistory()
        {
            this.ID = Guid.NewGuid();
            this.ModificationTime = DateTime.Now;
        }

        public TaskUpdateHistory(Guid user, string change, Guid task, string? comment)
        {
            this.ID = Guid.NewGuid();
            this.ModificationTime = DateTime.Now;
            this.User = user;
            this.Change = change;
            this.Task = task;
            this.Comment = comment;
        }

        public Guid ID { get; set; }
        public DateTime ModificationTime { get; }

        public Guid User { get; set; }
        public string Change { get; set; }

        public Guid Task { get; set; }

        public string? Comment { get; set; }

    }
}
