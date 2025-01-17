﻿namespace OJS.Services.Ui.Business;

using SoftUni.Services.Infrastructure;
using System.Threading.Tasks;
using SoftUni.Common.Models;
using OJS.Services.Ui.Models.Search;

public interface ISearchBusinessService : IService
{
    Task<PagedResult<ContestSearchServiceModel>> GetContestSearchResults(SearchServiceModel model);

    Task<PagedResult<ProblemSearchServiceModel>> GetProblemSearchResults(SearchServiceModel model);

    Task<PagedResult<UserSearchServiceModel>> GetUserSearchResults(SearchServiceModel model);
}