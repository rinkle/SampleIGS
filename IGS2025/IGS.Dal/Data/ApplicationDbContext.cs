using IGS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IGS.Dal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Home> Homes { get; set; } = default!;
        public DbSet<ErrorLog> ErrorLogs { get; set; } = default!; 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Home>()
            .   HasOne(h => h.CreatedByUser)
                .WithMany()
                .HasForeignKey(h => h.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Home>()
                .HasOne(h => h.ModifiedByUser)
                .WithMany()
                .HasForeignKey(h => h.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ErrorLog>().ToTable("ErrorLog");
        }

    }
}