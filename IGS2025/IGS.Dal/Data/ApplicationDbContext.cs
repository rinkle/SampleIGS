using IGS.Models;
using IGS.Models.KeyLessModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IGS.Dal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)  // ✅ correct constructor
        {
        }

        // Main entities
        public DbSet<Home> Homes { get; set; } = default!;
        public DbSet<CommonListing> CommonListings { get; set; } = default!;
        public DbSet<ErrorLog> ErrorLogs { get; set; } = default!;
        public DbSet<PageHeader> PageHeaders { get; set; } = default!;
        public DbSet<Page> Pages { get; set; } = default!;
        public DbSet<TransactionService> TransactionServices { get; set; } = default!;
        public DbSet<PortfolioService> PortfolioServices { get; set; } = default!;
        public DbSet<Industry> Industries { get; set; } = default!;
        public DbSet<IndustryCategory> IndustryCategories { get; set; } = default!;

        // New tables
        public DbSet<Experience> Experiences { get; set; } = default!;
        public DbSet<ExperienceIndustryMapping> ExperienceIndustryMappings { get; set; } = default!;
        public DbSet<ExperiencePageMapping> ExperiencePageMappings { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
        public DbSet<TeamLocation> TeamLocations { get; set; } = default!;
        public DbSet<TeamTitle> TeamTitles { get; set; } = default!;
        public DbSet<TeamCategory> TeamCategories { get; set; } = default!;
        public DbSet<TeamCategoryMapping> TeamCategoryMappings { get; set; } = default!;

        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsCategoryMapping> NewsCategoryMappings { get; set; }
        public DbSet<NewsCommonData> NewsCommonData { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Table mapping
            builder.Entity<Home>().ToTable("Home");
            builder.Entity<ErrorLog>().ToTable("ErrorLog");
            builder.Entity<PageHeader>().ToTable("PageHeader");
            builder.Entity<TransactionService>().ToTable("TransactionServices");
            builder.Entity<PortfolioService>().ToTable("PortfolioServices");
            builder.Entity<Industry>().ToTable("Industry");
            builder.Entity<IndustryCategory>().ToTable("IndustryCategory");
            builder.Entity<Experience>().ToTable("Experience");
            builder.Entity<ExperienceIndustryMapping>().ToTable("ExperienceIndustryMapping");
            builder.Entity<ExperiencePageMapping>().ToTable("ExperiencePageMapping");
            // ✅ Team table mappings
            builder.Entity<Team>().ToTable("Team");
            builder.Entity<TeamLocation>().ToTable("TeamLocation");
            builder.Entity<TeamTitle>().ToTable("TeamTitle");
            builder.Entity<TeamCategory>().ToTable("TeamCategory");
            builder.Entity<TeamCategoryMapping>().ToTable("TeamCategoryMapping");

            //news

            builder.Entity<News>().ToTable("News");
            builder.Entity<NewsCategory>().ToTable("NewsCategory");
            builder.Entity<NewsCategoryMapping>().ToTable("NewsCategoryMapping");
            builder.Entity<NewsPageMapping>().ToTable("NewsPageMapping");
            builder.Entity<NewsCommonData>().ToTable("NewsCommonData");

            builder.Entity<NewsCategory>().Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<NewsCategoryMapping>().Property(p => p.DisplayOrder).HasPrecision(18, 2);


            // Decimal precision
            builder.Entity<CommonListing>().Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<Experience>().Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<ExperienceIndustryMapping>().Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<ExperiencePageMapping>().Property(p => p.DisplayOrder).HasPrecision(18, 2);

            // Keyless SP result models
            builder.Entity<GetHome_Result>().HasNoKey().ToView(null);
            builder.Entity<GetCommonListing_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetPageHeader_Result>().HasNoKey().ToView(null);
            builder.Entity<GetTransactionService_Result>().HasNoKey().ToView(null);
            builder.Entity<GetPortfolioService_Result>().HasNoKey().ToView(null);
            builder.Entity<GetIndustry_Result>().HasNoKey().ToView(null);
            builder.Entity<GetIndustryCategory_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetExperienceDetail_Result>().HasNoKey().ToView(null);
            builder.Entity<GetExperienceFilterList_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetExperienceIndustryCategoryMapping_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetExperiencePageMapping_Result>().HasNoKey().ToView(null);

            builder.Entity<GetTeamFilterList_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetTeamDetails_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetTeamLocation_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<GetTeamCategory_Result>().HasNoKey().ToView(null)
                .Property(p => p.DisplayOrder).HasPrecision(18, 2);
            builder.Entity<getTeamTeamCategoryMapping_Result>().HasNoKey().ToView(null);

            builder.Entity<GetTeamTitle_Result>().HasNoKey().ToView(null);

            builder.Entity<GetNewsDetail_Result>().HasNoKey();
            builder.Entity<GetNewsFilterList_Result>().HasNoKey();
            builder.Entity<GetNewsCategoryMapping_Result>().HasNoKey();
            builder.Entity<GetNewsPageMapping_Result>().HasNoKey();
            builder.Entity<GetNewsCommonData_Result>().HasNoKey().ToView(null);

        }
    }
}
