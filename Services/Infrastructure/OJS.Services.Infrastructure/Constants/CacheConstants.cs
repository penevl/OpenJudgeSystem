﻿namespace OJS.Services.Infrastructure.Constants
{
    public static class CacheConstants
    {
        public const int OneHourInSeconds = 60 * 60;
        public const int OneDayInSeconds = 86400;
        public const int TwoMinutesInSeconds = 120;
        public const int FiveMinutesInSeconds = 300;

        public const string MainContestCategoriesDropDown = "MainContestCategoriesDropDown";
        public const string ContestResults = "Contest:{0}:official:{1}:full:{2}";
        public const string PracticeContestParticipantsCount = "ContestPartcipantsCount:Practice:{0}";
        public const string CompeteContestParticipantsCount = "ContestPartcipantsCount:Compete:{0}";
        public const string ContestCategoriesTree = "ContestCategoriesTree";

        public const string ContestSubCategoriesFormat = "ContestSubCategories_id_{0}";
        public const string ContestParentCategoriesFormat = "ContestParentCategories_id_{0}";
        public const string ContestCategoryNameFormat = "ContestCategoryName_id_{0}";

        public const string SubmissionsCountByMonthsForPastElevenMonthsKey = "Submissions_count_by_month_for_past_eleven_months";
        public const string SubmissionsCountForLastMonthKey = "Submissions_count_by_month_for_last_month";
        public const string TotalSubmissionsCount = "TotalSubmissionsCount";

        public const string SubmissionTypesByUsage = "SubmissionTypesByUsage";
    }
}