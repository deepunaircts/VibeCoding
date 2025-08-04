using System.Collections.Generic;
using System.Threading.Tasks;
using MemberEligibilityProcessor.Models;

namespace MemberEligibilityProcessor.Services
{
    public interface IEligibilityService
    {
        Task<IEnumerable<MemberEligibilityReport>> GenerateEligibilityReport();
    }

    public class MemberEligibilityReport
    {
        public string MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string MedicaidId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string GroupId { get; set; }
        public string PlanId { get; set; }
        public string ProductId { get; set; }
        public bool IsEligible { get; set; }
        public string EligibilityStatus { get; set; }
    }
}
