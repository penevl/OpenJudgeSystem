namespace OJS.Services.Administration.Data.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OJS.Common.Enumerations;
    using OJS.Data;
    using OJS.Data.Models.Contests;
    using OJS.Data.Models.Problems;
    using OJS.Services.Common;
    using OJS.Services.Common.Data.Implementations;
    using OJS.Services.Common.Models.Contests;
    using OJS.Services.Infrastructure;
    using SoftUni.AutoMapper.Infrastructure.Extensions;
    using SoftUni.AutoMapper.Infrastructure.Models;

    public class ContestsDataService : DataService<Contest>, IContestsDataService
    {
        private readonly IDatesService dates;
        private readonly IContestsActivityService activityService;

        public ContestsDataService(OjsDbContext db, IDatesService dates, IContestsActivityService activityService)
            : base(db)
        {
            this.dates = dates;
            this.activityService = activityService;
        }

        public async Task<IEnumerable<TServiceModel>> GetAllCompetable<TServiceModel>()
            where TServiceModel : IMapFrom<Contest>
            => await this.GetAllVisibleQuery()
                .Where(c =>
                    c.StartTime <= this.dates.GetUtcNow() &&
                    c.EndTime.HasValue &&
                    c.EndTime >= this.dates.GetUtcNow())
                .MapCollection<TServiceModel>()
                .ToListAsync();

        public async Task<IEnumerable<TServiceModel>> GetAllPast<TServiceModel>()
            where TServiceModel : IMapFrom<Contest>
            => await this.GetAllVisibleQuery()
                .Where(c => c.EndTime < this.dates.GetUtcNow())
                .MapCollection<TServiceModel>()
                .ToListAsync();

        public Task<Contest?> GetByIdWithProblems(int id)
            => this.DbSet
                .Include(c => c.ProblemGroups)
                    .ThenInclude(pg => pg.Problems)
                .FirstOrDefaultAsync(c => c.Id == id);

        public Task<Contest?> GetByIdWithParticipants(int id)
            => this.GetByIdQuery(id)
                .Include(c => c.Participants)
                .FirstOrDefaultAsync();

        public IQueryable<Contest> GetAllActive()
            => this.GetAllVisible()
                .Where(c =>
                    c.StartTime <= DateTime.Now &&
                    (c.EndTime >= DateTime.Now ||
                        (c.Type == ContestType.OnlinePracticalExam && c.Participants.Any(p =>
                            p.IsOfficial &&
                            p.ParticipationEndTime >= DateTime.Now))));

        public IQueryable<Contest> GetAllInactive()
            => this.DbSet
                .Where(c =>
                    c.StartTime > DateTime.Now ||
                    (c.EndTime < DateTime.Now && c.Type != ContestType.OnlinePracticalExam) ||
                    !c.Participants.Any(p => p.ParticipationEndTime < DateTime.Now));

        public IQueryable<Contest> GetAllUpcoming()
            => this.GetAllVisible()
                .Where(c => c.StartTime > DateTime.Now);

        public IQueryable<Contest> GetAllVisible()
            => this.DbSet
                .Where(c => c.IsVisible);

        public IQueryable<Contest> GetAllVisibleByCategory(int categoryId)
            => this.GetAllVisible()
                .Where(c => c.CategoryId == categoryId);

        public IQueryable<Contest> GetAllVisibleBySubmissionType(int submissionTypeId)
            => this.GetAllVisible()
                .Where(c => c.ProblemGroups
                    .SelectMany(pg => pg.Problems)
                    .Any(p => p.SubmissionTypesInProblems.Any(s => s.SubmissionTypeId == submissionTypeId)));

        public IQueryable<Contest> GetAllByLecturer(string? lecturerId)
            => this.DbSet
                .Where(c =>
                    c.LecturersInContests.Any(l => l.LecturerId == lecturerId) ||
                    c.Category!.LecturersInContestCategories.Any(l => l.LecturerId == lecturerId));

        public IQueryable<Contest> GetAllVisibleByCategoryAndLecturer(int categoryId, string? lecturerId)
            => this.GetAllByLecturer(lecturerId)
                .Where(c => c.CategoryId == categoryId);

        public IQueryable<Contest> GetContestWithIps(int id)
            => this.GetByIdQuery(id)
                .Include(c => c.IpsInContests)
                    .ThenInclude(ip => ip.Ip);

        public IQueryable<Contest> GetAllWithDeleted() => this.DbSet.IgnoreQueryFilters();

        public Task<int> GetMaxPointsById(int id)
            => this.GetMaxPointsByIdAndProblemGroupsFilter(id, pg => true);

        public Task<int> GetMaxPointsForExportById(int id)
            => this.GetMaxPointsByIdAndProblemGroupsFilter(id, pg => pg.Type != ProblemGroupType.ExcludedFromHomework);

        public async Task<string?> GetNameById(int id)
            => await this.GetByIdQuery(id)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

        public async Task<bool> IsActiveById(int id)
        {
            var contest = await this.OneById(id);

            if (contest == null)
            {
                return false;
            }

            return await this.activityService.IsContestActive(contest.Map<ContestForActivityServiceModel>());
        }

        public async Task<bool> IsOnlineById(int id)
            => await this.GetByIdQuery(id)
                .Select(c => c.Type)
                .FirstOrDefaultAsync() == ContestType.OnlinePracticalExam;

        public Task<bool> IsUserLecturerInContestByContestAndUser(int id, string? userId)
            => this.GetByIdQuery(id)
                .AnyAsync(c =>
                    c.LecturersInContests.Any(l => l.LecturerId == userId) ||
                    c.Category!.LecturersInContestCategories.Any(l => l.LecturerId == userId));

        public Task<bool> IsUserParticipantInByContestAndUser(int id, string? userId)
            => this.DbSet
                .AnyAsync(c =>
                    c.Id == id &&
                    c.Participants.Any(p => p.UserId == userId));

        public Task<bool> IsUserInExamGroupByContestAndUser(int id, string? userId)
            => this.DbSet
                .AnyAsync(c =>
                    c.Id == id &&
                    c.ExamGroups.Any(eg => eg.UsersInExamGroups.Any(u => u.UserId == userId)));

        private async Task<int> GetMaxPointsByIdAndProblemGroupsFilter(int id, Expression<Func<ProblemGroup, bool>> filter)
        {
            var problemsMaxPoints = await this.GetByIdQuery(id)
                .SelectMany(c => c.ProblemGroups)
                .Where(filter)
                .Select(pg => pg.Problems
                    .Where(p => !p.IsDeleted)
                    .Select(p => (int)p.MaximumPoints)
                    .FirstOrDefault())
                .ToListAsync();

            return problemsMaxPoints.Sum();
        }

        private IQueryable<Contest> GetAllVisibleQuery()
            => this.DbSet
                .Where(c => c.IsVisible);
    }
}