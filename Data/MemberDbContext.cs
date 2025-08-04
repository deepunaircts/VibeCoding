using Microsoft.EntityFrameworkCore;
using MemberEligibilityProcessor.Models;

namespace MemberEligibilityProcessor.Data
{
    public class MemberDbContext : DbContext
    {
        public MemberDbContext(DbContextOptions<MemberDbContext> options) 
            : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<MemberAddress> MemberAddresses { get; set; }
        public DbSet<MemberEligibility> MemberEligibilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.Addresses)
                .WithOne(a => a.Member)
                .HasForeignKey(a => a.MemberId);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.Eligibilities)
                .WithOne(e => e.Member)
                .HasForeignKey(e => e.MemberId);
        }
    }
}
