namespace OJS.Data.Models
{
    using OJS.Data.Infrastructure.Models;
    using System.Collections.Generic;

    public class Tag : DeletableAuditInfoEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ForegroundColor { get; set; }

        public string BackgroundColor { get; set; }

        public ICollection<TagInProblem> TagsInProblems { get; set; } = new HashSet<TagInProblem>();
    }
}