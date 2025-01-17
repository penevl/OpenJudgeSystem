﻿namespace OJS.Services.Ui.Models.Contests;

using AutoMapper;
using OJS.Data.Models.Contests;
using OJS.Services.Common.Models.Contests;
using SoftUni.AutoMapper.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using OJS.Services.Ui.Models.Submissions;

public class ContestDetailsServiceModel : IMapExplicitly, ICanBeCompetedAndPracticed
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool CanViewResults { get; set; }

    public bool IsOnlineExam { get; set; }

    public bool CanBeCompeted { get; set; }

    public bool CanBePracticed { get; set; }

    public bool IsAdminOrLecturerInContest { get; set; }

    public int CompeteParticipantsCount { get; set; }

    public int PracticeParticipantsCount { get; set; }

    public int? CategoryId { get; set; }

    public ICollection<ContestDetailsSubmissionTypeServiceModel> AllowedSubmissionTypes { get; set; } =
        new HashSet<ContestDetailsSubmissionTypeServiceModel>();

    public ICollection<ContestProblemServiceModel> Problems { get; set; } = new HashSet<ContestProblemServiceModel>();

    public void RegisterMappings(IProfileExpression configuration) =>
        configuration.CreateMap<Contest, ContestDetailsServiceModel>()
            .ForMember(
                d => d.Problems,
                opt => opt.MapFrom(s =>
                    s.ProblemGroups
                        .SelectMany(pg => pg.Problems)
                        .OrderBy(p => p.ProblemGroup.OrderBy)
                        .ThenBy(p => p.OrderBy)))
            .ForMember(d => d.IsAdminOrLecturerInContest, opt => opt.Ignore())
            .ForMember(d => d.CanViewResults, opt => opt.Ignore())
            .ForMember(d => d.AllowedSubmissionTypes, opt => opt.Ignore())
            .ForMember(d => d.CompeteParticipantsCount, opt => opt.Ignore())
            .ForMember(d => d.PracticeParticipantsCount, opt => opt.Ignore())
            .ForMember(d => d.CanBeCompeted, opt => opt.Ignore())
            .ForMember(d => d.CanBePracticed, opt => opt.Ignore());
}