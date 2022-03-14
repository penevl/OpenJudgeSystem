﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OJS.Services.Ui.Business;

namespace OJS.Servers.Ui.Controllers.Api;

public class ProblemResources : Controller
{
    private IProblemResourcesBusinessService problemResourcesBusinessService;

    public ProblemResources(IProblemResourcesBusinessService problemResourcesBusinessService)
        => this.problemResourcesBusinessService = problemResourcesBusinessService;

    [HttpGet]
    [Authorize]
    public async Task<FileContentResult> GetResource(int id)
    {
        var resource = await this.problemResourcesBusinessService.GetResource(id);

        Response.Headers.Add("Content-Disposition", $"attachment; filename*=UTF-8''{Uri.EscapeDataString(resource.Name)}.{resource.FileExtension}");
        return File(resource.File, "application/octet-stream");
    }
}