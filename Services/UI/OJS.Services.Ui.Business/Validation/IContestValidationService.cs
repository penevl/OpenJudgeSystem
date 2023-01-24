﻿namespace OJS.Services.Ui.Business.Validation;

using OJS.Data.Models.Contests;
using OJS.Services.Common.Validation;

public interface IContestValidationService : IValidationServiceAsync<(Contest, string, bool, bool)>
{
}