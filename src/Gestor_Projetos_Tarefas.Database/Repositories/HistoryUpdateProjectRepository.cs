using Gestor_Projetos_Tarefas.Database.Context;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Domain.Models;
using Gestor_Projetos_Tarefas.Domain.Models.Enums;

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

        public async Task<List<Guid>> ReturnHistoryByUser(Guid userID)
        {
            return (from record in dbContext.History
                    join projectTask in dbContext.Tasks on record.Task equals projectTask.ID
                    where record.User == userID && record.ModificationTime >= DateTime.Now.AddDays(-30) && projectTask.Status == ProjectTaskStatus.Concluida
                    select projectTask.ID
                 ).ToList();
        }
    }
}
