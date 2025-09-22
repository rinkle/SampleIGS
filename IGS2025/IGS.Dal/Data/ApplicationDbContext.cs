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
        public DbSet<TransactionService> TransactionServices { get; set; } = default!;
        public DbSet<PortfolioService> PortfolioServices { get; set; } = default!;
        public DbSet<Industry> Industries { get; set; } = default!;
        public DbSet<IndustryCategory> IndustryCategories { get; set; } = default!;

        //Keyless entities (SP result models)
        //public DbSet<GetHome_Result> GetHomeResults { get; set; } = default!;
        //public DbSet<GetCommonListing_Result> GetCommonListingResults { get; set; } = default!;
        //public DbSet<GetPageHeader_Result> GetPageHeaders { get; set; } = default!;
        //public DbSet<GetTransactionService_Result> GetTransactionServices { get; set; } = default!;
        //public DbSet<GetPortfolioService_Result> GetPortfolioService { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Home>().ToTable("Home");
            builder.Entity<ErrorLog>().ToTable("ErrorLog");
            builder.Entity<PageHeader>().ToTable("PageHeader");
            builder.Entity<TransactionService>().ToTable("TransactionServices");
            builder.Entity<PortfolioService>().ToTable("PortfolioServices");

            builder.Entity<Industry>().ToTable("Industry");
            builder.Entity<IndustryCategory>().ToTable("IndustryCategory");

            builder.Entity<CommonListing>()
                .Property(p => p.DisplayOrder)
                .HasPrecision(18, 2);

            // Keyless entities (SP result models) → explicitly NOT mapped to tables/views
            builder.Entity<GetHome_Result>().HasNoKey().ToView(null);
            builder.Entity<GetCommonListing_Result>().HasNoKey().ToView(null).Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetPageHeader_Result>().HasNoKey().ToView(null);
            builder.Entity<GetTransactionService_Result>().HasNoKey().ToView(null);
            builder.Entity<GetPortfolioService_Result>().HasNoKey().ToView(null);
            builder.Entity<GetIndustry_Result>().HasNoKey().ToView(null);
            builder.Entity<GetIndustryCategory_Result>().HasNoKey().ToView(null);
        }
    }
}