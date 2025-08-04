using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemberRequestAPI.Models
{
    public class MemberRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public string MemberId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        
        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zipcode { get; set; }

        [Required]
        public string RequestType { get; set; } // Welcome Kit, Handbook, ID Card

        [Required]
        public string Language { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public DateTime? ProcessedDate { get; set; }
        public DateTime? MailingDate { get; set; }
        public string RequestFileName { get; set; }
        public string VendorAckFileName { get; set; }
        public DateTime? VendorAckDate { get; set; }
    }
}
