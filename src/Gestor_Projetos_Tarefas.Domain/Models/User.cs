using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.Models
{
    [Table("users")]
    public class User
    {
        public User()
        {
            this.ID = Guid.NewGuid();
        }

        public User(string name, Role role, string email, List<Guid> projects)
        {
            this.ID = Guid.NewGuid();
            this.Name = name;
            this.Role = role;
            this.Email = email;
            this.Projects = projects;
        }

        public Guid ID { get; set; }
        public String Name { get; set; }
        public  Role Role { get; set; }

        public String Email {  get; set; }
        public List<Guid> Projects { get; set; }
    }
}
