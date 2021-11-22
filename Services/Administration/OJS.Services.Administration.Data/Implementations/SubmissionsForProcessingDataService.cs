﻿using Microsoft.EntityFrameworkCore;
using OJS.Data.Models.Submissions;
using OJS.Services.Common.Data.Implementations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OJS.Services.Administration.Data.Implementations
{
    public class SubmissionsForProcessingDataService : DataService<SubmissionForProcessing>, ISubmissionsForProcessingDataService
    {
        public SubmissionsForProcessingDataService(DbContext submissionsForProcessing) : base(submissionsForProcessing) {}

        public Task<SubmissionForProcessing> GetBySubmission(int submissionId) =>
            this.DbSet
                .Where(s => s.SubmissionId == submissionId)
                .FirstOrDefaultAsync();

        public IQueryable<SubmissionForProcessing> GetAllUnprocessed() =>
            this.DbSet
                .Where(sfp => !sfp.Processed && !sfp.Processing);

        public async Task<IEnumerable<int>> GetIdsOfAllProcessing() =>
            await this.DbSet
                .Where(sfp => sfp.Processing && !sfp.Processed)
                .Select(sfp => sfp.Id)
                .ToListAsync();

        // public Task AddOrUpdateBySubmissionIds(ICollection<int> submissionIds)
        // {
        //     var newSubmissionsForProcessing = submissionIds
        //         .Select(sId => new SubmissionForProcessing
        //         {
        //             SubmissionId = sId
        //         });
        //
        //     using (var scope = TransactionsHelper.CreateTransactionScope())
        //     {
        //         submissionIds
        //             .Chunk(GlobalConstants.BatchOperationsChunkSize)
        //             .ForEach(chunk => base.Delete(sfp => chunk.Contains(sfp.SubmissionId)));
        //
        //         base.AddMany(newSubmissionsForProcessing);
        //
        //         scope.Complete();
        //     }
        //
        //     return Task.CompletedTask;
        // }

        public async Task AddOrUpdateBySubmission(int submissionId)
        {
            var submissionForProcessing = await this.GetBySubmission(submissionId);

            if (submissionForProcessing != null)
            {
                submissionForProcessing.Processing = false;
                submissionForProcessing.Processed = false;
            }
            else
            {
                submissionForProcessing = new SubmissionForProcessing
                {
                    SubmissionId = submissionId
                };

                await base.Add(submissionForProcessing);
                await base.SaveChanges();
            }
        }

        public async Task RemoveBySubmission(int submissionId)
        {
            var submissionForProcessing = this.GetBySubmission(submissionId);

            if (submissionForProcessing != null)
            {
                await base.DeleteById(submissionId);
                await base.SaveChanges();
            }
        }

        public async Task ResetProcessingStatusById(int id)
        {
            var submissionForProcessing = await base.OneById(id);
            if (submissionForProcessing != null)
            {
                submissionForProcessing.Processing = false;
                submissionForProcessing.Processed = false;
                await base.SaveChanges();
            }
        }

        public void Clean() =>
            this.DbSet.RemoveRange(this.DbSet.Where(sfp => sfp.Processed && !sfp.Processing));

        public async Task Update(SubmissionForProcessing submissionForProcessing)
        {
            base.Update(submissionForProcessing);
            await base.SaveChanges();
        }
    }
}