namespace OJS.Services.Administration.Business.Validation.Helpers.Implementations;

using OJS.Services.Administration.Data;
using OJS.Services.Common;
using OJS.Services.Common.Models;
using System.Threading.Tasks;
using AdminResource = OJS.Common.Resources.AdministrationGeneral;

public class ContestCategoriesValidationHelper : IContestCategoriesValidationHelper
{
    private readonly IContestCategoriesDataService contestCategoriesData;
    private readonly Business.IUserProviderService userProvider;

    public ContestCategoriesValidationHelper(
        IContestCategoriesDataService contestCategoriesData,
        Business.IUserProviderService userProvider)
    {
        this.contestCategoriesData = contestCategoriesData;
        this.userProvider = userProvider;
    }

    public async Task<ValidationResult> ValidatePermissionsOfCurrentUser(int contestCategoryId)
    {
        var user = this.userProvider.GetCurrentUser();

        var userHasPermissionsForCategory = await this.contestCategoriesData
            .UserHasContestCategoryPermissions(contestCategoryId, user.Id, user.IsAdmin);

        return userHasPermissionsForCategory
            ? ValidationResult.Valid()
            : ValidationResult.Invalid(AdminResource.NoPrivilegesMessage);
    }
}