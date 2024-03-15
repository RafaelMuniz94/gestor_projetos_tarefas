using Gestor_Projetos_Tarefas.Domain.Models;

namespace Gestor_Projetos_Tarefas.Api.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<bool> ChangeTaskUser(Guid oldUserID, Guid newUserID, Guid projectID);
        public Task<User> ReturnUser(Guid userID);
    }
}
