﻿namespace OJS.Services.Ui.Models.Submissions
{
    using OJS.Workers.Common;
    using SoftUni.AutoMapper.Infrastructure.Models;

    public class CheckerDetailsResponseModel : IMapFrom<CheckerDetails>
    {
        public string? Comment { get; set; }

        public string? ExpectedOutputFragment { get; set; }

        public string? UserOutputFragment { get; set; }
    }
}