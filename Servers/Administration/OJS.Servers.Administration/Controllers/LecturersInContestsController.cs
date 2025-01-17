namespace OJS.Servers.Administration.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoCrudAdmin.Enumerations;
using AutoCrudAdmin.Extensions;
using AutoCrudAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OJS.Common;
using Microsoft.Extensions.Options;
using OJS.Data.Models;
using OJS.Data.Models.Users;
using OJS.Services.Administration.Data;
using OJS.Services.Administration.Models;
using static OJS.Common.GlobalConstants.Roles;

[Authorize(Roles = Administrator)]
public class LecturersInContestsController : BaseAutoCrudAdminController<LecturerInContest>
{
    private const string ContestName = nameof(LecturerInContest.Contest);

    private readonly IUsersDataService usersDataService;

    public LecturersInContestsController(
        IUsersDataService usersDataService,
        IOptions<ApplicationConfig> appConfigOptions)
        : base(appConfigOptions)
        => this.usersDataService = usersDataService;

    protected override Expression<Func<LecturerInContest, bool>>? MasterGridFilter
        => this.GetMasterGridFilter();

    protected override IEnumerable<FormControlViewModel> GenerateFormControls(
        LecturerInContest entity,
        EntityAction action,
        IDictionary<string, string> entityDict,
        IDictionary<string, Expression<Func<object, bool>>> complexOptionFilters,
        Type autocompleteType)
    {
        var formControls = base.GenerateFormControls(entity, action, entityDict, complexOptionFilters, typeof(UserProfile)).ToList();
        formControls.Add(new FormControlViewModel()
        {
            Name = nameof(UserProfile.UserName),
            Options = this.usersDataService.GetQuery(take: GlobalConstants.NumberOfAutocompleteItemsShown).ToList(),
            FormControlType = FormControlType.Autocomplete,
            DisplayName = nameof(LecturerInContest.Lecturer),
            FormControlAutocompleteController = nameof(UsersController).ToControllerBaseUri(),
            FormControlAutocompleteEntityId = nameof(LecturerInContest.LecturerId),
        });

        var formControlToRemove = formControls.First(x =>
            x.DisplayName == nameof(LecturerInContest.Lecturer) && x.FormControlType != FormControlType.Autocomplete);
        formControls.Remove(formControlToRemove);

        return formControls;
    }

    protected override Expression<Func<LecturerInContest, bool>>? GetMasterGridFilter()
    {
        if (this.TryGetEntityIdForStringColumnFilter(ContestName, out var contestName))
        {
            return lc => lc.Contest.Name == contestName;
        }

        return base.MasterGridFilter;
    }
}