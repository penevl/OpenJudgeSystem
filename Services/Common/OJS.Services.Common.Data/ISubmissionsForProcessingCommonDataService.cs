namespace OJS.Services.Common.Data;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OJS.Data.Models.Submissions;
using OJS.Services.Common.Models.Submissions;

public interface ISubmissionsForProcessingCommonDataService : IDataService<SubmissionForProcessing>
{
    IQueryable<SubmissionForProcessing> GetAllPending();

    IQueryable<SubmissionForProcessing> GetAllUnprocessed();

    IQueryable<SubmissionForProcessing> GetAllProcessing();

    Task<SubmissionForProcessing> Add(int submissionId, string serializedExecutionDetails);

    Task<SubmissionForProcessing> AddOrUpdate(int submissionId, string serializedExecutionDetails);

    Task RemoveBySubmission(int submissionId);

    Task MarkProcessing(int submissionId);

    Task MarkMultipleForProcessing(ICollection<int> submissionsIds);

    Task MarkProcessed(SerializedSubmissionExecutionResultServiceModel submissionExecutionResult);
}