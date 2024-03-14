using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.Models
{
    [Table("projects")]
    public class Project
    {
        public Project()
        {
           this.ID = Guid.NewGuid(); 
        }
        public Project(string name, string description)
        {
            this.ID = Guid.NewGuid();
            this.Name = name;
            this.Description = description; 
        }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
