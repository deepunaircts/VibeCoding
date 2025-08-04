using System;
using System.Collections.Generic;

namespace MemberEligibilityProcessor.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MedicaidId { get; set; }
        public virtual ICollection<MemberAddress> Addresses { get; set; }
        public virtual ICollection<MemberEligibility> Eligibilities { get; set; }
    }
}
