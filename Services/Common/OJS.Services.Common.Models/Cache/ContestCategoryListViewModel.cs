﻿namespace OJS.Services.Common.Models.Cache;

using OJS.Common.Extensions.Strings;
using OJS.Data.Models.Contests;
using SoftUni.AutoMapper.Infrastructure.Models;

public class ContestCategoryListViewModel : IMapFrom<ContestCategory>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    public string NameUrl => this.Name.ToUrl();
}