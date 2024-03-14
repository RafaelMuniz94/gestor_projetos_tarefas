using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System;
using System.Collections.Generic;
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

        public ProjectTask(TasksStatus status, TaskPriority priority, string title, string description, DateTime expirationDate, Guid project, Guid user)
        {
            this.ID = Guid.NewGuid()
            this.Status = status;
            this.Priority = priority;
            this.Title = title;
            this.Description = description;
            this.ExpirationDate = expirationDate;
            this.Project = project;
            this.User = user;
        }

        public Guid ID { get; set; }
        public TasksStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Guid Project { get; set;}
        public Guid User { get; set;}
    }
}
