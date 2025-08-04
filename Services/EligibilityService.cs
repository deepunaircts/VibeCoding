using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberEligibilityProcessor.Data;
using MemberEligibilityProcessor.Models;
using Microsoft.EntityFrameworkCore;

namespace MemberEligibilityProcessor.Services
{
    public class EligibilityService : IEligibilityService
    {
        private readonly MemberDbContext _context;

        public EligibilityService(MemberDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MemberEligibilityReport>> GenerateEligibilityReport()
        {
            // Get the current date at midnight to ensure consistent date comparison
            var currentDate = DateTime.Today;
            
            // Get all active eligibilities as of current date
            var eligibleMembers = await _context.MemberEligibilities
                .Where(e => e.EligibilityIndicator && // Must be marked as eligible
                          e.EffectiveDate <= currentDate && // Effective on or before today
                          (e.TerminationDate == null || e.TerminationDate >= currentDate)) // Not terminated or terminates in the future
                .Include(e => e.Member)
                    .ThenInclude(m => m.Addresses)
                .ToListAsync();

            var report = new List<MemberEligibilityReport>();

            foreach (var eligibility in eligibleMembers)
            {
                var homeAddress = eligibility.Member.Addresses
                    .FirstOrDefault(a => a.AddressType.Equals("Home", StringComparison.OrdinalIgnoreCase));

                report.Add(new MemberEligibilityReport
                {
                    MemberId = eligibility.MemberId,
                    LastName = eligibility.Member.LastName,
                    FirstName = eligibility.Member.FirstName,
                    DateOfBirth = eligibility.Member.DateOfBirth.ToString("yyyy-MM-dd"),
                    MedicaidId = eligibility.Member.MedicaidId,
                    Address = $"{homeAddress?.AddressLine1} {homeAddress?.AddressLine2}".Trim(),
                    City = homeAddress?.City,
                    State = homeAddress?.State,
                    ZipCode = homeAddress?.ZipCode,
                    GroupId = eligibility.GroupId,
                    PlanId = eligibility.PlanId,
                    ProductId = eligibility.ProductId,
                    IsEligible = eligibility.EligibilityIndicator,
                    // Determine eligibility status based on dates
                    EligibilityStatus = IsCurrentlyEligible(eligibility, currentDate) 
                        ? "Active" 
                        : "Inactive"
                });
            }

            return report;
        }

        /// <summary>
        /// Determines if a member is currently eligible based on their eligibility record and the current date
        /// </summary>
        private bool IsCurrentlyEligible(MemberEligibility eligibility, DateTime currentDate)
        {
            // Check if eligibility is active
            if (!eligibility.EligibilityIndicator)
                return false;

            // Check effective date
            if (eligibility.EffectiveDate > currentDate)
                return false;

            // Check termination date (if exists)
            if (eligibility.TerminationDate.HasValue && eligibility.TerminationDate.Value < currentDate)
                return false;

            return true;
        }
    }
}
