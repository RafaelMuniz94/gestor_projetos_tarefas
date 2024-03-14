using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
