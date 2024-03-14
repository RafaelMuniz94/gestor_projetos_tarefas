﻿using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;


namespace Gestor_Projetos_Tarefas.Database.Repositories
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly DataBaseContext dbContext;

        public UsersRepository(DataBaseContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<bool?> RemoveProjectFromUser(Guid userID, Guid projectID)
        {
            User user = await dbContext.Users.FindAsync(userID);

            if (user == null) return null;

            bool removed = user.Projects.Remove(projectID);

            return removed;

        }
    }
}