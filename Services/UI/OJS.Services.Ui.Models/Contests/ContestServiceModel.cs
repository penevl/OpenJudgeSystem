﻿using OJS.Common.Enumerations;
using OJS.Services.Ui.Models.SubmissionTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OJS.Services.Ui.Models.Contests
{
    public class ContestServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? PracticeStartTime { get; set; }

        public DateTime? PracticeEndTime { get; set; }

        public int LimitBetweenSubmissions { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsVisible { get; set; }

        public bool IsOnline { get; set; }

        public string ContestPassword { private get; set; }

        public string PracticePassword { private get; set; }

        public bool HasContestQuestions { get; set; }

        public bool HasPracticeQuestions { get; set; }

        public int OfficialParticipants { get; set; }

        public int PracticeParticipants { get; set; }

        public int ProblemsCount { get; set; }

        public ContestType ContestType { get; set; }

        public IEnumerable<SubmissionTypeServiceModel> AllowedSubmissionTypes { get; set; }

        public IEnumerable<ContestProblemServiceModel> Problems { get; set; }

        public IEnumerable<ContestCategoryListViewModel> ParentCategories { get; set; } =
            Enumerable.Empty<ContestCategoryListViewModel>();

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

        public bool HasContestPassword => this.ContestPassword != null;

        public bool HasPracticePassword => this.PracticePassword != null;

        public double? RemainingTimeInMilliseconds
        {
            get
            {
                if (this.EndTime.HasValue)
                {
                    return (this.EndTime.Value - DateTime.Now).TotalMilliseconds;
                }

                return null;
            }
        }

        public bool UserIsAdminOrLecturerInContest { get; set; }

        public bool UserCanCompete { get; set; }

        public bool UserIsParticipant { get; set; }

        public bool IsActive { get; set; }
    }
}