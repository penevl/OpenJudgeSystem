﻿using OJS.Services.Common.Models.PubSubContracts.Submissions;

namespace OJS.Services.Worker.Models.ExecutionContext
{
    using System;
    using AutoMapper;
    using OJS.Services.Worker.Models.ExecutionContext.ExecutionDetails;
    using OJS.Services.Worker.Models.ExecutionContext.Mapping;
    using OJS.Workers.Common.Models;
    using OJS.Workers.SubmissionProcessors.Models;
    using SoftUni.AutoMapper.Infrastructure.Models;

    public class SubmissionServiceModel : IMapExplicitly
    {
        public ExecutionType ExecutionType { get; set; }

        public ExecutionStrategyType ExecutionStrategyType { get; set; }

        public byte[]? FileContent { get; set; }

        public string? Code { get; set; }

        public int TimeLimit { get; set; }

        public int MemoryLimit { get; set; }

        public SimpleExecutionDetailsServiceModel? SimpleExecutionDetails { get; set; }

        public TestsExecutionDetailsServiceModel? TestsExecutionDetails { get; set; }

        public ExecutionOptionsServiceModel ExecutionOptions { get; set; } = new ();

        public DateTime? StartedExecutionOn { get; set; }

        public void RegisterMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap(typeof(SubmissionServiceModel), typeof(OjsSubmission<>))
                .ForMember(
                    nameof(OjsSubmission<object>.ExecutionType),
                    opt => opt.MapFrom(typeof(ExecutionTypeValueResolver)))
                .ForMember(
                    nameof(OjsSubmission<object>.Input),
                    opt => opt.MapFrom(typeof(SubmissionInputValueResolver)))
                .ForMember(
                    nameof(OjsSubmission<object>.ExecutionStrategyType),
                    opt => opt.MapFrom(nameof(SubmissionServiceModel.ExecutionStrategyType)))
                .ForMember(
                    nameof(OjsSubmission<object>.Code),
                    opt => opt.MapFrom(nameof(SubmissionServiceModel.Code)))
                .ForMember(
                    nameof(OjsSubmission<object>.FileContent),
                    opt => opt.MapFrom(nameof(SubmissionServiceModel.FileContent)))
                .ForMember(
                    nameof(OjsSubmission<object>.MemoryLimit),
                    opt => opt.MapFrom(nameof(SubmissionServiceModel.MemoryLimit)))
                .ForMember(
                    nameof(OjsSubmission<object>.TimeLimit),
                    opt => opt.MapFrom(nameof(SubmissionServiceModel.TimeLimit)))
                .ForAllOtherMembers(opt => opt.Ignore());

            configuration
                .CreateMap<SubmissionSubmitted, SubmissionServiceModel>()
                .ForMember(m => m.ExecutionStrategyType, opt => opt.MapFrom(src => src.ExecutionStrategy))
                .ForMember(m => m.SimpleExecutionDetails, opt => opt.Ignore())
                .ForMember(m => m.ExecutionOptions, opt => opt.Ignore())
                .ForMember(m => m.StartedExecutionOn, opt => opt.Ignore());
        }
    }
}
