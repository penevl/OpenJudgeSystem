﻿namespace OJS.Services.Administration.Business.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentExtensions.Extensions;
    using OJS.Services.Administration.Data;
    using OJS.Services.Common;
    using OJS.Services.Common.Data;
    using OJS.Services.Common.Models.Submissions.ExecutionContext;
    using SoftUni.AutoMapper.Infrastructure.Extensions;

    public class SubmissionsForProcessingBusinessService : ISubmissionsForProcessingBusinessService
    {
        private readonly ISubmissionsCommonBusinessService submissionsCommonBusinessService;
        private readonly ISubmissionsForProcessingCommonDataService submissionsForProcessingData;
        private readonly ISubmissionsDataService submissionsData;
        private readonly ISubmissionPublisherService submissionPublisherService;

        public SubmissionsForProcessingBusinessService(
            ISubmissionsForProcessingCommonDataService submissionsForProcessingData,
            ISubmissionsDataService submissionsData,
            ISubmissionPublisherService submissionPublisherService,
            ISubmissionsCommonBusinessService submissionsCommonBusinessService)
        {
            this.submissionsForProcessingData = submissionsForProcessingData;
            this.submissionsData = submissionsData;
            this.submissionPublisherService = submissionPublisherService;
            this.submissionsCommonBusinessService = submissionsCommonBusinessService;
        }

        /// <summary>
        /// Sets the Processing property to False for all submissions
        /// thus ensuring that the worker will process them eventually instead
        /// of getting stuck in perpetual "Processing..." state.
        /// </summary>
        public async Task ResetAllProcessingSubmissions()
        {
            var allProcessingSubmissionIds = await this.submissionsForProcessingData.GetIdsOfAllProcessing();

            if (allProcessingSubmissionIds.Count() <= 0)
            {
                return;
            }

            foreach (var submissionForProcessingId in allProcessingSubmissionIds)
            {
                await this.submissionsForProcessingData.ResetProcessingStatusById(submissionForProcessingId);
            }
        }

        public void EnqueueStaleSubmissions()
        {
            var submissionsForProcessing = this.submissionsForProcessingData
                .GetAllUnprocessedAndNotProcessing()
                .ToList()
                .Where(sfp => Math.Abs(sfp!.CreatedOn.Subtract(DateTime.UtcNow).TotalMinutes) >= 1);

            if (!submissionsForProcessing.Any())
            {
                return;
            }

            var submissions = this.submissionsData
                .GetByIds(submissionsForProcessing
                    .Select(sp => sp!.SubmissionId)
                    .ToList())
                .ToList();

            submissions
                .ForEachSequential((submission) =>
                    this.submissionsCommonBusinessService
                    .PublishSubmissionForProcessing(submission.Map<SubmissionServiceModel>()))
                .GetAwaiter()
                .GetResult();
        }
    }
}