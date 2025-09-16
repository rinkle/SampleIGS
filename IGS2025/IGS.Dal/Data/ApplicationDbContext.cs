using IGS.Models;
using IGS.Models.KeyLessModels;
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
        public DbSet<CommonListing> CommonListings { get; set; } = default!;
        public DbSet<ErrorLog> ErrorLogs { get; set; } = default!;

        //Keyless entities (SP result models)
        public DbSet<GetHome_Result> GetHomeResults { get; set; } = default!;
        public DbSet<GetCommonListing_Result> GetCommonListingResults { get; set; } = default!;

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
            builder.Entity<GetHome_Result>().HasNoKey();
            builder.Entity<GetCommonListing_Result>().HasNoKey();

        }

    }
}