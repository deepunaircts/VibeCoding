using System.ComponentModel.DataAnnotations;

namespace MemberRequestAPI.Models.Dashboard
{
    public class MemberRequestDashboard
    {
        public int RequestId { get; set; }
        
        [Display(Name = "Member ID")]
        public string MemberId { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Address")]
        public string FullAddress { get; set; }

        [Display(Name = "Request Type")]
        public string RequestType { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Days Awaiting Ack")]
        public int DaysAwaitingAck { get; set; }

        [Display(Name = "Request File Name")]
        public string RequestFileName { get; set; }
    }
}
