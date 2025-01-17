﻿namespace OJS.Services.Administration.Data.Implementations;

using FluentExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OJS.Common;
using OJS.Data.Models.Tests;
using OJS.Services.Common.Data.Implementations;

public class TestRunsDataService : DataService<TestRun>, ITestRunsDataService
{
    public TestRunsDataService(DbContext testRuns)
        : base(testRuns)
    {
    }

    public IQueryable<TestRun> GetAllByTest(int testId) =>
        this.DbSet
            .Where(tr => tr.TestId == testId);

    public async Task DeleteByProblem(int problemId)
    {
        this.Delete(tr => tr.Test.ProblemId == problemId);
        await this.SaveChanges();
    }

    public async Task DeleteByTest(int testId)
    {
        this.Delete(tr => tr.TestId == testId);
        await this.SaveChanges();
    }

    public async Task DeleteBySubmission(int submissionId)
    {
        this.Delete(tr => tr.SubmissionId == submissionId);
        await this.SaveChanges();
    }

    public async Task DeleteInBatchesBySubmissionIds(IEnumerable<int> submissionIds)
        => await submissionIds
            .Chunk(GlobalConstants.BatchOperationsChunkSize)
            .ForEachSequential(async chunk =>
            {
                this.Delete(t => chunk.Contains(t.SubmissionId));

                await this.SaveChanges();
            });
}