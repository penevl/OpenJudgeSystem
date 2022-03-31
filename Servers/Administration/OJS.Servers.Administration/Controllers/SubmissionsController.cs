namespace OJS.Servers.Administration.Controllers;

using AutoCrudAdmin.Extensions;
using AutoCrudAdmin.Models;
using OJS.Data.Models.Problems;
using OJS.Data.Models.Submissions;
using OJS.Services.Administration.Business;
using OJS.Services.Administration.Business.Validation.Helpers;
using OJS.Services.Administration.Data;
using OJS.Services.Infrastructure.Exceptions;
using OJS.Services.Infrastructure.Extensions;
using SoftUni.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class SubmissionsController : BaseAutoCrudAdminController<Submission>
{
    public const string ContestIdKey = nameof(ProblemGroup.ContestId);
    public const string ProblemIdKey = nameof(Submission.ProblemId);
    public const string ParticipantIdKey = nameof(Submission.ParticipantId);

    private readonly IProblemsValidationHelper problemsValidationHelper;
    private readonly IParticipantScoresDataService participantScoresData;
    private readonly IParticipantScoresBusinessService participantScoresBusiness;
    private readonly ISubmissionsDataService submissionsData;
    private readonly ISubmissionsForProcessingDataService submissionsForProcessingData;
    private readonly ITestRunsDataService testRunsData;
    private readonly ITransactionsProvider transactions;

    public SubmissionsController(
        IProblemsValidationHelper problemsValidationHelper,
        IParticipantScoresDataService participantScoresData,
        IParticipantScoresBusinessService participantScoresBusiness,
        ISubmissionsDataService submissionsData,
        ISubmissionsForProcessingDataService submissionsForProcessingData,
        ITestRunsDataService testRunsData,
        ITransactionsProvider transactions)
    {
        this.problemsValidationHelper = problemsValidationHelper;
        this.participantScoresData = participantScoresData;
        this.participantScoresBusiness = participantScoresBusiness;
        this.submissionsData = submissionsData;
        this.submissionsForProcessingData = submissionsForProcessingData;
        this.testRunsData = testRunsData;
        this.transactions = transactions;
    }

    protected override Expression<Func<Submission, bool>>? MasterGridFilter
        => GetMasterGridFilter();

    protected override IEnumerable<CustomGridColumn<Submission>> CustomColumns
        => new CustomGridColumn<Submission>[]
        {
            new()
            {
                Name = "Contest",
                ValueFunc = s => s.Problem != null ? s.Problem.ProblemGroup.Contest.ToString() : string.Empty,
            },
            new()
            {
                Name = "Contest Id",
                ValueFunc = s => s.Problem != null ? s.Problem!.ProblemGroup.ContestId.ToString() : string.Empty,
            },
        };

    protected override async Task BeforeEntitySaveAsync(Submission submission, AdminActionContext actionContext)
    {
        await this.problemsValidationHelper
            .ValidatePermissionsOfCurrentUser(submission.ProblemId ?? default)
            .VerifyResult();

        if (!submission.ParticipantId.HasValue)
        {
            throw new BusinessServiceException("The participant does not exist.");
        }
    }

    protected override async Task DeleteEntityAndSaveAsync(Submission submission, AdminActionContext actionContext)
    {
        var submissionProblemId = submission.ProblemId!.Value;
        var submissionParticipantId = submission.ParticipantId!.Value;

        await this.transactions.ExecuteInTransaction(async () =>
        {
            await this.testRunsData.DeleteBySubmission(submission.Id);
            await this.submissionsData.DeleteById(submission.Id);
            await this.submissionsData.SaveChanges();
            await this.submissionsForProcessingData.RemoveBySubmission(submission.Id);

            var isBestSubmission = await this.IsBestSubmission(
                submissionProblemId,
                submissionParticipantId,
                submission.Id);

            if (isBestSubmission)
            {
                await this.participantScoresBusiness.RecalculateForParticipantByProblem(
                    submission.ParticipantId.Value,
                    submission.ProblemId.Value);
            }
        });
    }

    private Expression<Func<Submission, bool>>? GetMasterGridFilter()
    {
        if (this.TryGetEntityIdForNumberColumnFilter(ContestIdKey, out var contestId))
        {
            return x => x.Problem != null && x.Problem.ProblemGroup.ContestId == contestId;
        }

        if (this.TryGetEntityIdForNumberColumnFilter(ProblemIdKey, out var problemId))
        {
            return x => x.ProblemId == problemId;
        }

        if (this.TryGetEntityIdForNumberColumnFilter(ParticipantIdKey, out var participantId))
        {
            return x => x.ParticipantId == participantId;
        }

        return base.MasterGridFilter;
    }

    private async Task<bool> IsBestSubmission(int problemId, int participantId, int submissionId)
    {
        var bestScore = await this.participantScoresData.GetByParticipantIdAndProblemId(participantId, problemId);

        return bestScore?.SubmissionId == submissionId;
    }
}