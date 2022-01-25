namespace OJS.Servers.Administration.Controllers;

using AutoCrudAdmin.Models;
using AutoCrudAdmin.ViewModels;
using OJS.Common.Extensions;
using OJS.Data.Models.Contests;
using OJS.Services.Administration.Business;
using OJS.Services.Administration.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Resource = OJS.Common.Resources.ExamGroupsController;

public class ExamGroupsController : BaseAutoCrudAdminController<ExamGroup>
{
    private readonly IContestsBusinessService contestsBusiness;
    private readonly IContestsDataService contestsData;

    public ExamGroupsController(
        IContestsBusinessService contestsBusiness,
        IContestsDataService contestsData)
    {
        this.contestsBusiness = contestsBusiness;
        this.contestsData = contestsData;
    }

    protected override IEnumerable<Func<ExamGroup, ExamGroup, AdminActionContext, Task<ValidatorResult>>> AsyncEntityValidators
        => new Func<ExamGroup, ExamGroup, AdminActionContext, Task<ValidatorResult>>[]
        {
            this.ValidateContestPermissions,
        };

    private async Task<ValidatorResult> ValidateContestPermissions(
        ExamGroup existingEntity,
        ExamGroup newEntity,
        AdminActionContext actionContext)
    {
        var userId = this.User.GetId();
        var isUserAdmin = this.User.IsAdmin();

        if (!newEntity.ContestId.HasValue)
        {
            return ValidatorResult.Success();
        }

        if (!await this.contestsBusiness.UserHasContestPermissions(newEntity.ContestId.Value, userId, isUserAdmin))
        {
            return ValidatorResult.Error(Resource.Cannot_attach_contest);
        }

        if (actionContext.Action == EntityAction.Delete)
        {
            if (await this.contestsData.IsActiveById(newEntity.ContestId.Value))
            {
                return ValidatorResult.Error(Resource.Cannot_delete_group_with_active_contest);
            }
        }

        return ValidatorResult.Success();
    }
}