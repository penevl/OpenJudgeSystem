﻿namespace OJS.Servers.Ui.Controllers.Api;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJS.Common.Enumerations;
using OJS.Servers.Infrastructure.Controllers;
using OJS.Servers.Infrastructure.Extensions;
using OJS.Servers.Ui.Models;
using OJS.Servers.Ui.Models.Submissions.Details;
using OJS.Servers.Ui.Models.Submissions.Profile;
using OJS.Services.Common.Models.Submissions;
using OJS.Services.Ui.Business;
using OJS.Services.Ui.Business.Cache;
using OJS.Services.Ui.Models.Submissions;
using SoftUni.AutoMapper.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static OJS.Common.GlobalConstants.Roles;

public class SubmissionsController : BaseApiController
{
    private readonly ISubmissionsBusinessService submissionsBusiness;
    private readonly ISubmissionsForProcessingBusinessService submissionsForProcessingBusiness;
    private readonly ISubmissionCacheService submissionCache;

    public SubmissionsController(
        ISubmissionsBusinessService submissionsBusiness,
        ISubmissionsForProcessingBusinessService submissionsForProcessingBusiness,
        ISubmissionCacheService submissionCache)
    {
        this.submissionsBusiness = submissionsBusiness;
        this.submissionsForProcessingBusiness = submissionsForProcessingBusiness;
        this.submissionCache = submissionCache;
    }

    /// <summary>
    /// Gets all user submissions. Prepared for the user's profile page.
    /// </summary>
    /// <param name="username">Username of the profile's owner.</param>
    /// <param name="page">The current page number.</param>
    /// <returns>A page with submissions containing information about their score and user.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SubmissionForProfileResponseModel>), Status200OK)]
    public async Task<IActionResult> GetForProfile([FromQuery] string username, [FromQuery]int page)
        => await this.submissionsBusiness
            .GetForProfileByUser(username, page)
            .Map<PagedResultResponse<SubmissionForProfileResponseModel>>()
            .ToOkResult();

    /// <summary>
    /// Gets user latest submissions for contest.
    /// </summary>
    /// /// <param name="username">Username of the profile's owner.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="contestId">Contest for which the submissions will be retrieved.</param>
    /// <returns>A page with submissions containing information about their score and user.</returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedResultResponse<SubmissionForProfileResponseModel>), Status200OK)]
    public async Task<IActionResult> GetUserSubmissionsForProfileByContest([FromQuery] string username, [FromQuery] int page, [FromQuery] int contestId)
        => await this.submissionsBusiness
            .GetForProfileByUserAndContest(username, page, contestId)
            .Map<PagedResultResponse<SubmissionForProfileResponseModel>>()
            .ToOkResult();

    /// <summary>
    /// Gets the submitted file.
    /// </summary>
    /// <param name="id">Id of the submission.</param>
    /// <returns>The file to download.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FileContentResult), Status200OK)]
    public IActionResult Download(int id)
    {
        var submissionDownloadServiceModel = this.submissionsBusiness.GetSubmissionFile(id);

        return this.File(submissionDownloadServiceModel.Content!, submissionDownloadServiceModel.MimeType!, submissionDownloadServiceModel.FileName);
    }

    /// <summary>
    /// Gets a subset of submissions by specific problem.
    /// </summary>
    /// <param name="id">The id of the problem.</param>
    /// <param name="isOfficial">Should the submissions be only from compete mode.</param>
    /// <param name="page">Current submissions page.</param>
    /// <returns>A collection of submissions for a specific problem.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PagedResultResponse<SubmissionResultsResponseModel>), Status200OK)]
    public async Task<IActionResult> GetSubmissionResultsByProblem(
        int id,
        [FromQuery] bool isOfficial,
        [FromQuery] int page)
        => await this.submissionsBusiness
            .GetSubmissionResultsByProblem(id, isOfficial, page)
            .Map<PagedResultResponse<SubmissionResultsResponseModel>>()
            .ToOkResult();

    /// <summary>
    /// Gets a subset of submission results for the selected user by specific problem in details page.
    /// </summary>
    /// <param name="submissionId">The id of the submission.</param>
    /// <param name="page">Current submissions page.</param>
    /// <returns>A collection of submissions for a specific problem.</returns>
    [HttpGet("{submissionId:int}")]
    [ProducesResponseType(typeof(PagedResultResponse<SubmissionResultsResponseModel>), Status200OK)]
    public async Task<IActionResult> GetSubmissionResults(
        int submissionId,
        [FromQuery] int page)
        => await this.submissionsBusiness
            .GetSubmissionResults(submissionId, page)
            .Map<PagedResultResponse<SubmissionResultsResponseModel>>()
            .ToOkResult();

    /// <summary>
    /// Gets submission details by provided submission id.
    /// </summary>
    /// <param name="id">The id of the submission.</param>
    /// <returns>Submission details model.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SubmissionDetailsResponseModel), Status200OK)]
    public async Task<IActionResult> Details(int id)
        => await this.submissionsBusiness
            .GetDetailsById(id)
            .Map<SubmissionDetailsResponseModel>()
            .ToOkResult();

    /// <summary>
    /// Saves/updates the provided execution result for the given submission in the database.
    /// </summary>
    /// <param name="submissionExecutionResult">The submission execution result.</param>
    /// <returns>Success model.</returns>
    /// <remarks>
    /// The submission comes from the RabbitMQ execution result queue.
    /// It sends it to here after executing it on a remote worker.
    /// </remarks>
    [HttpPost("/Submissions/SaveExecutionResult")]
    [ProducesResponseType(typeof(SaveExecutionResultResponseModel), Status200OK)]
    public async Task<IActionResult> SaveExecutionResult([FromBody] SubmissionExecutionResult submissionExecutionResult)
    {
        await this.submissionsBusiness.ProcessExecutionResult(submissionExecutionResult);

        var result = new SaveExecutionResultResponseModel { SubmissionId = submissionExecutionResult.SubmissionId, };

        return this.Ok(result);
    }

    /// <summary>
    /// Gets the count of all submissions.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(int), Status200OK)]
    public async Task<IActionResult> TotalCount()
        => await this.submissionCache
            .GetTotalCount()
            .ToOkResult();

    /// <summary>
    /// Gets the count of all unprocessed submissions.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = Administrator)]
    [ProducesResponseType(typeof(int), Status200OK)]
    public async Task<IActionResult> UnprocessedTotalCount()
        => await this.submissionsForProcessingBusiness
            .GetUnprocessedTotalCount()
            .ToOkResult();

    // Unify (Public, GetProcessingSubmissions, GetPendingSubmissions) endpoints for Submissions into single one.
    [ProducesResponseType(typeof(PagedResultResponse<SubmissionForPublicSubmissionsResponseModel>), Status200OK)]
    public async Task<IActionResult> GetSubmissions([FromQuery] SubmissionStatus status, [FromQuery] int page)
         => await this.submissionsBusiness.GetSubmissions(status, page)
             .Map<PagedResultResponse<SubmissionForPublicSubmissionsResponseModel>>()
             .ToOkResult();
}