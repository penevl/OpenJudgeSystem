namespace OJS.Servers.Administration.Models.Problems
{
    using System;
    using System.Linq.Expressions;

    using OJS.Data.Models.Problems;

    public class ProblemModel
    {
        public static Expression<Func<Problem, ProblemModel>> Model =>
            p => new ProblemModel
            {
                Id = p.Id,
                Name = p.Name,
                MaximumPoints = p.MaximumPoints,
                TimeLimit = p.TimeLimit,
                MemoryLimit = p.MemoryLimit,
            };

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public short MaximumPoints { get; set; }

        public int TimeLimit { get; set; }

        public int MemoryLimit { get; set; }
    }
}