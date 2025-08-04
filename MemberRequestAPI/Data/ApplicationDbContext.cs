using Microsoft.EntityFrameworkCore;
using MemberRequestAPI.Models;

namespace MemberRequestAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MemberRequest> MemberRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MemberRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.Property(e => e.MemberId).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.Zipcode).IsRequired();
                entity.Property(e => e.RequestType).IsRequired();
                entity.Property(e => e.Language).IsRequired();
            });
        }
    }
}
