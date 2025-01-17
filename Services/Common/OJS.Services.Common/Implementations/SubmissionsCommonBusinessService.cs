namespace OJS.Services.Common.Implementations;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using OJS.Services.Common.Data;
using OJS.Services.Common.Models.Submissions.ExecutionContext;
using Microsoft.Extensions.Logging;

public class SubmissionsCommonBusinessService : ISubmissionsCommonBusinessService
{
    private readonly ISubmissionPublisherService submissionPublisherService;
    private readonly ISubmissionsForProcessingCommonDataService submissionForProcessingData;
    private readonly ILogger<SubmissionsCommonBusinessService> logger;

    public SubmissionsCommonBusinessService(
        ISubmissionPublisherService submissionPublisherService,
        ISubmissionsForProcessingCommonDataService submissionForProcessingData,
        ILogger<SubmissionsCommonBusinessService> logger)
    {
        this.submissionPublisherService = submissionPublisherService;
        this.submissionForProcessingData = submissionForProcessingData;
        this.logger = logger;
    }

    public async Task PublishSubmissionForProcessing(SubmissionServiceModel submission)
    {
        try
        {
            await this.submissionPublisherService.Publish(submission);

            await this.submissionForProcessingData.MarkProcessing(submission.Id);
        }
        catch (Exception ex)
        {
            this.logger.LogError($"Exception in submitting solution {submission.Id} by {Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.InnerException}");
            throw;
        }
    }

    public async Task PublishSubmissionsForProcessing(IEnumerable<SubmissionServiceModel> submissions)
    {
        try
        {
            await this.submissionPublisherService.PublishMultiple(submissions);

            var submissionsIds = submissions.Select(s => s.Id).ToList();

            await this.submissionForProcessingData.MarkMultipleForProcessing(submissionsIds);
        }
        catch (Exception ex)
        {
            this.logger.LogError($"Exception in submitting solution {submissions} by {Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.InnerException}");
            throw;
        }
    }
}