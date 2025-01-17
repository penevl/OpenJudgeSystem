﻿namespace OJS.Servers.Ui.Models.Submissions.Compete;

using AutoMapper;
using OJS.Services.Ui.Models.Submissions;
using SoftUni.AutoMapper.Infrastructure.Models;

public class SubmissionRequestModel : IMapExplicitly
{
    public int ProblemId { get; set; }

    public int SubmissionTypeId { get; set; }

    public string? Content { get; set; }

    public bool Official { get; set; }

    public void RegisterMappings(IProfileExpression configuration)
        => configuration.CreateMap<SubmissionRequestModel, SubmitSubmissionServiceModel>()
            .ForMember(
                d => d.StringContent,
                opt => opt.MapFrom(s =>
                    s.Content == null
                        ? null
                        : s.Content))
            .ForMember(d => d.ByteContent, opt => opt.Ignore())
            .ForMember(d => d.FileExtension, opt => opt.Ignore());
}