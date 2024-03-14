using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.Models
{
    [Table("tasks")]
    public class ProjectTask
    {
        public ProjectTask()
        {
            this.ID = Guid.NewGuid();
        }

        public ProjectTask(ProjectTaskStatus status, TaskPriority priority, string title, string description, DateTime expirationDate, Guid project, Guid user)
        {
            this.ID = Guid.NewGuid();
            this.Status = status;
            this.Priority = priority;
            this.Title = title;
            this.Description = description;
            this.ExpirationDate = expirationDate;
            this.Project = project;
            this.User = user;
        }

        [Key]
        public Guid ID { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("projects")]
        public Guid Project { get; set;}
        
        [ForeignKey("users")]
        public Guid User { get; set;}
    }
}
