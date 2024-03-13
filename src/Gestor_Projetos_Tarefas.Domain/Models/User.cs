using Gestor_Projetos_Tarefas.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Domain.Models
{
    internal class User
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public  Role Role { get; set; }

        public String Email {  get; set; }
        public List<Project> Projects { get; set; }
    }
}
