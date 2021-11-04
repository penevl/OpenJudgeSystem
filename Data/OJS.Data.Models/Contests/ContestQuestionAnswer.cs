namespace OJS.Data.Models.Contests
{
    using OJS.Data.Infrastructure.Models;
    using System.ComponentModel.DataAnnotations;
    using static OJS.Data.Validation.ConstraintConstants.Contest;

    public class ContestQuestionAnswer : DeletableAuditInfoEntity<int>
    {
        public int QuestionId { get; set; }

        public virtual ContestQuestion Question { get; set; }

        [MaxLength(QuestionAnswerMaxLength)]
        [MinLength(QuestionAnswerMinLength)]
        public string Text { get; set; }
    }
}