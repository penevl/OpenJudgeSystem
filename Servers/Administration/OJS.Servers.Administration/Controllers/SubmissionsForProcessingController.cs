﻿namespace OJS.Servers.Administration.Controllers;

using AutoCrudAdmin.ViewModels;
using System.Collections.Generic;
using System.Linq;
using OJS.Data.Models.Submissions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OJS.Common;
using OJS.Servers.Administration.Extensions;
using Microsoft.Extensions.Options;
using OJS.Services.Administration.Models;

public class SubmissionsForProcessingController : BaseAutoCrudAdminController<SubmissionForProcessing>
{
    public SubmissionsForProcessingController(IOptions<ApplicationConfig> appConfigOptions)
        : base(appConfigOptions)
    {
    }

    protected override IEnumerable<GridAction> CustomActions
        => new[] { new GridAction { Action = nameof(this.Details) } };

    protected override IEnumerable<GridAction> DefaultActions
        => Enumerable.Empty<GridAction>();

    public override Task<IActionResult> PostEdit(IDictionary<string, string> complexId, FormFilesContainer files)
    {
        this.TempData.AddDangerMessage(Resources.AdministrationGeneral.CannotEditSubmissionForProcessing);
        return Task.FromResult<IActionResult>(this.RedirectToAction("Index", "SubmissionsForProcessing"));
    }

    public Task<IActionResult> Details([FromQuery] IDictionary<string, string> complexId) => this.Edit(complexId, nameof(this.PostEdit));
}