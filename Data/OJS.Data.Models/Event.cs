namespace OJS.Data.Models
{
    using SoftUni.Data.Infrastructure.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Event : DeletableAuditInfoEntity<int>
    {
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }

        /// <remarks>
        /// If EndTime is null, the event happens (and should be displayed) only on the StartTime.
        /// </remarks>
        public DateTime? EndTime { get; set; }

        public string? Url { get; set; }
    }
}