namespace OJS.Data.Models.Contests
{
    using OJS.Common.Enumerations;
    using OJS.Data.Infrastructure.Models;
    using OJS.Data.Models.Participants;
    using OJS.Data.Models.Problems;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using static OJS.Data.Validation.ConstraintConstants.Contest;

    public class Contest : DeletableEntity<int>, IOrderableEntity
    {
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; }

        public bool IsVisible { get; set; }

        public bool AutoChangeTestsFeedbackVisibility { get; set; }

        public int? CategoryId { get; set; }

        public ContestCategory Category { get; set; }

        public ContestType Type { get; set; }

        /// <summary>
        /// Gets or sets the duration of online contest in which a participant can compete
        /// </summary>
        /// <remarks>
        /// If duration is null the actual duration is the difference between
        /// start and end time of the contest
        /// </remarks>
        public TimeSpan? Duration { get; set; }

        /// <remarks>
        /// If StartTime is null the contest cannot be competed.
        /// </remarks>
        public DateTime? StartTime { get; set; }

        /// <remarks>
        /// If EndTime is null the contest can be competed forever.
        /// </remarks>
        public DateTime? EndTime { get; set; }

        /// <remarks>
        /// If ContestPassword is null the contest can be competed by everyone without require a password.
        /// If the ContestPassword is not null the contest participant should provide a valid password.
        /// </remarks>
        [MaxLength(PasswordMaxLength)]
        public string ContestPassword { get; set; }

        /// <remarks>
        /// If PracticePassword is null the contest can be practiced by everyone without require a password.
        /// If the PracticePassword is not null the practice participant should provide a valid password.
        /// </remarks>
        [MaxLength(PasswordMaxLength)]
        public string PracticePassword { get; set; }

        /// <remarks>
        /// NewIpPassword is user for allowing a new IP to be used for the contest.
        /// </remarks>
        [MaxLength(20)]
        public string NewIpPassword { get; set; }

        /// <remarks>
        /// If PracticeStartTime is null the contest cannot be practiced.
        /// </remarks>
        public DateTime? PracticeStartTime { get; set; }

        /// <remarks>
        /// If PracticeEndTime is null the contest can be practiced forever.
        /// </remarks>
        public DateTime? PracticeEndTime { get; set; }

        public int LimitBetweenSubmissions { get; set; }

        public double OrderBy { get; set; }

        public short NumberOfProblemGroups { get; set; }

        public string Description { get; set; }

        public ICollection<LecturerInContest> LecturersInContests { get; set; } = new HashSet<LecturerInContest>();

        public ICollection<ProblemGroup> ProblemGroups { get; set; } = new HashSet<ProblemGroup>();

        public ICollection<Participant> Participants { get; set; } = new HashSet<Participant>();

        public ICollection<IpInContest> IpsInContests { get; set; } = new HashSet<IpInContest>();

        public ICollection<ExamGroup> ExamGroups { get; set; } = new HashSet<ExamGroup>();

        [NotMapped]
        public bool CanBeCompeted
        {
            get
            {
                if (!this.IsVisible)
                {
                    return false;
                }

                if (this.IsDeleted)
                {
                    return false;
                }

                if (!this.StartTime.HasValue)
                {
                    // Cannot be competed
                    return false;
                }

                if (!this.EndTime.HasValue)
                {
                    // Compete forever
                    return this.StartTime <= DateTime.Now;
                }

                return this.StartTime <= DateTime.Now && DateTime.Now <= this.EndTime;
            }
        }

        [NotMapped]
        public bool CanBePracticed
        {
            get
            {
                if (!this.IsVisible)
                {
                    return false;
                }

                if (this.IsDeleted)
                {
                    return false;
                }

                if (!this.PracticeStartTime.HasValue)
                {
                    // Cannot be practiced
                    return false;
                }

                if (!this.PracticeEndTime.HasValue)
                {
                    // Practice forever
                    return this.PracticeStartTime <= DateTime.Now;
                }

                return this.PracticeStartTime <= DateTime.Now && DateTime.Now <= this.PracticeEndTime;
            }
        }

        [NotMapped]
        public bool IsActive => this.CanBeCompeted ||
            (this.Type == ContestType.OnlinePracticalExam &&
                this.Participants.Any(p =>
                    p.IsOfficial &&
                    p.ParticipationEndTime.HasValue &&
                    p.ParticipationEndTime.Value >= DateTime.Now));

        [NotMapped]
        public bool ResultsArePubliclyVisible
        {
            get
            {
                if (!this.IsVisible)
                {
                    return false;
                }

                if (this.IsDeleted)
                {
                    return false;
                }

                if (!this.StartTime.HasValue)
                {
                    // Cannot be competed
                    return false;
                }

                return this.EndTime.HasValue && this.EndTime.Value <= DateTime.Now;
            }
        }

        [NotMapped]
        public bool HasContestPassword => this.ContestPassword != null;

        [NotMapped]
        public bool HasPracticePassword => this.PracticePassword != null;

        [NotMapped]
        public bool IsOnline => this.Type == ContestType.OnlinePracticalExam;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            var contest = validationContext.ObjectInstance as Contest;
            if (contest == null)
            {
                return validationResults;
            }

            if (contest.StartTime.HasValue && contest.EndTime.HasValue && contest.StartTime.Value > contest.EndTime.Value)
            {
                validationResults.Add(
                    new ValidationResult("StartTime can not be after EndTime", new[] { "StartTime", "EndTime" }));
            }

            return validationResults;
        }

        public override string ToString() => $"#{this.Id} {this.Name}";
    }
}