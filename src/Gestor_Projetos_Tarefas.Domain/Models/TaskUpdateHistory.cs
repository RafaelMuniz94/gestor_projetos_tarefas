using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace Gestor_Projetos_Tarefas.Domain.Models
{
    [Table("taskUpdateHistory")]
    [ExcludeFromCodeCoverage]
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

        [Key]
        public Guid ID { get; set; }
        public DateTime ModificationTime { get; set; }

        [ForeignKey("users")]
        public Guid User { get; set; }
        public string Change { get; set; }

        [ForeignKey("tasks")]
        public Guid Task { get; set; }

        public string? Comment { get; set; }

    }
}
