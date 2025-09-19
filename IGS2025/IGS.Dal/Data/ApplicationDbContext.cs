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
        public DbSet<PageHeader> PageHeaders { get; set; } = default!;
        public DbSet<Page> Pages { get; set; } = default!;

        //Keyless entities (SP result models)
        public DbSet<GetHome_Result> GetHomeResults { get; set; } = default!;
        public DbSet<GetCommonListing_Result> GetCommonListingResults { get; set; } = default!;
        public DbSet<GetPageHeader_Result> GetPageHeaders { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Home>().ToTable("Home");
            builder.Entity<ErrorLog>().ToTable("ErrorLog");
            builder.Entity<PageHeader>().ToTable("PageHeader");

            builder.Entity<GetHome_Result>().HasNoKey();
            builder.Entity<GetCommonListing_Result>().HasNoKey();
            builder.Entity<GetPageHeader_Result>().HasNoKey();


        }

    }
}