using System.Threading.Tasks;

namespace OJS.Services.Ui.Data.Implementations
{
    using Microsoft.EntityFrameworkCore;
    using OJS.Data;
    using OJS.Data.Models.Submissions;
    using OJS.Services.Common.Data.Implementations;
    using System;
    using System.Linq;

    public class SubmissionsDataService : DataService<Submission>, ISubmissionsDataService
    {
        public SubmissionsDataService(OjsDbContext db) : base(db)
        {
        }

        public Submission GetBestForParticipantByProblem(int participantId, int problemId) =>
            this.GetAllByProblemAndParticipant(problemId, participantId)
                .Where(s => s.Processed)
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.Id)
                .FirstOrDefault();

        public IQueryable<Submission> GetByIdQuery(int id) =>
            this.DbSet
                .Where(s => s.Id == id);

        public IQueryable<Submission> GetAllByProblem(int problemId)
            => base.DbSet.Where(s => s.ProblemId == problemId);

        public IQueryable<Submission> GetAllByProblemAndParticipant(int problemId, int participantId) =>
            this.DbSet
                .Where(s => s.ParticipantId == participantId && s.ProblemId == problemId);

        public IQueryable<Submission> GetAllFromContestsByLecturer(string lecturerId) =>
            this.DbSet
                .Include(s => s.Problem.ProblemGroup.Contest.LecturersInContests)
                .Include(s => s.Problem.ProblemGroup.Contest.Category.LecturersInContestCategories)
                .Where(s =>
                    (s.IsPublic.HasValue && s.IsPublic.Value) ||
                    s.Problem.ProblemGroup.Contest.LecturersInContests.Any(l => l.LecturerId == lecturerId) ||
                    s.Problem.ProblemGroup.Contest.Category.LecturersInContestCategories.Any(l =>
                        l.LecturerId == lecturerId));

        public IQueryable<Submission> GetAllCreatedBeforeDateAndNonBestCreatedBeforeDate(
            DateTime createdBeforeDate,
            DateTime nonBestCreatedBeforeDate) =>
            this.DbSet
                .Where(s => s.CreatedOn < createdBeforeDate ||
                            (s.CreatedOn < nonBestCreatedBeforeDate &&
                             s.Participant.Scores.All(ps => ps.SubmissionId != s.Id)));

        public IQueryable<Submission> GetAllHavingPointsExceedingLimit()
            => this.DbSet
                .Where(s => s.Points > s.Problem.MaximumPoints);

        public IQueryable<int> GetIdsByProblem(int problemId)
            => this.GetAllByProblem(problemId)
                .Select(s => s.Id);

        public bool IsOfficialById(int id) =>
            this.GetByIdQuery(id)
                .Any(s => s.Participant.IsOfficial);

        public void SetAllToUnprocessedByProblem(int problemId) =>
            this.GetAllByProblem(problemId)
                .UpdateFromQueryAsync(s => new Submission
                {
                    Processed = false
                });

        public void DeleteByProblem(int problemId) =>
            this.DbSet.RemoveRange(this.DbSet.Where(s => s.ProblemId == problemId));

        public void RemoveTestRunsCacheByProblem(int problemId) =>
            this.GetAllByProblem(problemId)
                .UpdateFromQueryAsync(s => new Submission
                {
                    TestRunsCache = null
                });

        public bool HasSubmissionTimeLimitPassedForParticipant(int participantId, int limitBetweenSubmissions)
        {
            var lastSubmission =
                this.DbSet
                    .Where(s => s.ParticipantId == participantId)
                    .OrderByDescending(s => s.CreatedOn)
                    .Select(s => new { s.Id, s.CreatedOn })
                    .FirstOrDefault();

            if (lastSubmission != null)
            {
                // check if the submission was sent after the submission time limit has passed
                var latestSubmissionTime = lastSubmission.CreatedOn;
                var differenceBetweenSubmissions = DateTime.Now - latestSubmissionTime;
                if (differenceBetweenSubmissions.TotalSeconds < limitBetweenSubmissions)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasUserNotProcessedSubmissionForProblem(int problemId, string userId) =>
            this.DbSet.Any(s => s.ProblemId == problemId && s.Participant.UserId == userId && !s.Processed);
    }
}