﻿namespace OJS.Services.Ui.Models.Submissions;

using System.Collections.Generic;

public class SubmissionDetailsWithResultsModel
{
    public SubmissionDetailsServiceModel? SubmissionDetails { get; set; }
    public IEnumerable<SubmissionViewInResultsPageModel>? SubmissionResults { get; set; }
}