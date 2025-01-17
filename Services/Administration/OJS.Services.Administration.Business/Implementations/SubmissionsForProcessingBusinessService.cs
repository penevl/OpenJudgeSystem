﻿namespace OJS.Services.Administration.Business.Implementations;

using System.Threading.Tasks;
using FluentExtensions.Extensions;
using Microsoft.EntityFrameworkCore;
using OJS.Services.Common;
using OJS.Services.Common.Data;
using OJS.Services.Common.Models.Submissions.ExecutionContext;
using SoftUni.AutoMapper.Infrastructure.Extensions;

public class SubmissionsForProcessingBusinessService : ISubmissionsForProcessingBusinessService
{
    private readonly ISubmissionsCommonBusinessService submissionsCommonBusinessService;
    private readonly ISubmissionsForProcessingCommonDataService submissionsForProcessingData;
    private readonly ISubmissionsCommonDataService submissionsCommonData;

    public SubmissionsForProcessingBusinessService(
        ISubmissionsForProcessingCommonDataService submissionsForProcessingData,
        ISubmissionsCommonDataService submissionsCommonData,
        ISubmissionsCommonBusinessService submissionsCommonBusinessService)
    {
        this.submissionsForProcessingData = submissionsForProcessingData;
        this.submissionsCommonData = submissionsCommonData;
        this.submissionsCommonBusinessService = submissionsCommonBusinessService;
    }

    public async Task<int> EnqueuePendingSubmissions()
    {
        var pendingSubmissions = await this.submissionsCommonData
            .GetAllPending()
            .Include(s => s.SubmissionType)
            .MapCollection<SubmissionServiceModel>()
            .ToListAsync();

        if (pendingSubmissions.IsEmpty())
        {
            return 0;
        }

        await this.submissionsCommonBusinessService.PublishSubmissionsForProcessing(pendingSubmissions);

        return pendingSubmissions.Count;
    }

    public async Task DeleteProcessedSubmissions()
    {
        this.submissionsForProcessingData.Delete(sfp => sfp.Processed && !sfp.Processing);
        await this.submissionsForProcessingData.SaveChanges();
    }
}