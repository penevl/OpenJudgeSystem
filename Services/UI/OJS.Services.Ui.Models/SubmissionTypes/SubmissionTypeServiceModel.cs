namespace OJS.Services.Ui.Models.SubmissionTypes;

using System.Collections.Generic;
using AutoMapper;
using OJS.Data.Models.Submissions;
using SoftUni.AutoMapper.Infrastructure.Models;

public class SubmissionTypeServiceModel : IMapExplicitly
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsSelectedByDefault { get; set; }

    public bool AllowBinaryFilesUpload { get; set; }

    public IEnumerable<string> AllowedFileExtensions { get; set; } = new List<string>();

    public void RegisterMappings(IProfileExpression configuration)
        => configuration.CreateMap<SubmissionType, SubmissionTypeServiceModel>()
            .ForMember(
                d => d.AllowedFileExtensions,
                opt => opt.MapFrom(s => s.AllowedFileExtensionsList));
}
