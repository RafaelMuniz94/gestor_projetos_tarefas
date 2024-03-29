﻿using Gestor_Projetos_Tarefas.Domain.Models;


namespace Gestor_Projetos_Tarefas.Database.Interfaces
{
    public interface IHistoryUpdateProjectRepository
    {
        Task<bool> AddHistoryRecord(TaskUpdateHistory record);
        Task<List<Guid>> ReturnHistoryByUser(Guid userID, DateTime dateLimit);
    }
}
