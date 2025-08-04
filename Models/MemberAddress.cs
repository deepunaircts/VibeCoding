namespace MemberEligibilityProcessor.Models
{
    public class MemberAddress
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string AddressType { get; set; } // 'Home', 'Mailing', etc.
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public virtual Member Member { get; set; }
    }
}
