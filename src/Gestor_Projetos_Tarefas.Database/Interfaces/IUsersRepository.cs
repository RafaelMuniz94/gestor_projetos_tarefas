using Gestor_Projetos_Tarefas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_Projetos_Tarefas.Database.Interfaces
{
    internal interface IUsersRepository
    {
        Task<bool?> RemoveProjectFromUser(Guid userID,Guid projectID);
    }
}
