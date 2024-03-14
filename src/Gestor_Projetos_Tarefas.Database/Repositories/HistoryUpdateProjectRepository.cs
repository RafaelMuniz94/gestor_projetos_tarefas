using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;


namespace Gestor_Projetos_Tarefas.Database.Repositories
{
    public class HistoryUpdateProjectRepository: IHistoryUpdateProjectRepository
    {
        private readonly DataBaseContext dbContext;

        public HistoryUpdateProjectRepository(DataBaseContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<bool> AddHistoryRecord(TaskUpdateHistory record)
        {
            dbContext.History.AddAsync(record);
            int dbResponse = dbContext.SaveChanges();

            return dbResponse > 0; 
        }
    }
}
