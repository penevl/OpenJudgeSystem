namespace OJS.Services.Administration.Business.Validation.Implementations.ExamGroups;

using OJS.Services.Administration.Models.ExamGroups;
using OJS.Services.Common.Models;
using OJS.Services.Common.Validation;
using Resource = OJS.Common.Resources.ExamGroupsController;

public class UsersInExamGroupsCreateDeleteValidationService
    : IValidationService<UserInExamGroupCreateDeleteValidationServiceModel>
{
    public ValidationResult GetValidationResult(UserInExamGroupCreateDeleteValidationServiceModel? item)
    {
        var contestId = item!.ContestId;

        if (contestId.HasValue)
        {
            return ValidationResult.Valid();
        }

        var message = item.IsCreate
            ? Resource.Cannot_add_users
            : Resource.Cannot_remove_users;

        return ValidationResult.Invalid(message);
    }
}