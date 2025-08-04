using System;

namespace MemberEligibilityProcessor.Models
{
    public class MemberEligibility
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string PlanId { get; set; }
        public string ProductId { get; set; }
        public string GroupId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool EligibilityIndicator { get; set; }
        public virtual Member Member { get; set; }
    }
}
