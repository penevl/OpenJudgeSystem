namespace OJS.Services.Ui.Business.Validations.Implementations.Contests;

using OJS.Data.Models.Contests;
using OJS.Services.Common;
using OJS.Services.Common.Models;
using OJS.Services.Common.Validation.Helpers;

public class ContestResultsValidationService : IContestResultsValidationService
{
    private readonly INotDefaultValueValidationHelper notDefaultValueValidationHelper;
    private readonly IUserProviderService userProvider;

    public ContestResultsValidationService(
        INotDefaultValueValidationHelper notDefaultValueValidationHelper,
        IUserProviderService userProvider)
    {
        this.notDefaultValueValidationHelper = notDefaultValueValidationHelper;
        this.userProvider = userProvider;
    }

    public ValidationResult GetValidationResult((Contest?, bool) item)
    {
        var (contest, fullResults) = item;

        var contestNullValidationResult = this.notDefaultValueValidationHelper
            .ValidateValueIsNotDefault(contest, nameof(contest));

        if (!contestNullValidationResult.IsValid)
        {
            return contestNullValidationResult;
        }

        var user = this.userProvider.GetCurrentUser();

        if (fullResults && !user.IsAdminOrLecturer)
        {
            return ValidationResult.Invalid(ValidationMessages.Participant.NoPrivilegesForContestResults);
        }

        return ValidationResult.Valid();
    }
}