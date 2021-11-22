﻿namespace OJS.Services.Ui.Business
{
    using OJS.Data.Models.Submissions;
    using SoftUni.Services.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ISubmissionsBusinessService : IService
    {
        Task<IQueryable<Submission>> GetAllForArchiving();

        Task RecalculatePointsByProblem(int problemId);

        // Task HardDeleteAllArchived();
    }
}