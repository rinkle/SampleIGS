namespace IGS.Models.KeyLessModels
{
    // DTO for stored procedure "GetHome"
    public class GetHome_Result
    {
        public int Id { get; set; }
        public string? TransactionsGrowthHeading { get; set; }
        public string? TransactionsGrowthDescription { get; set; }
        public string? CoreAreasHeading { get; set; }
        public string? CoreAreaDescription { get; set; }
        public string? RecentProjectsHeading { get; set; }
        public string? RecentProjectsDescription { get; set; }
        public string? InsightTitle { get; set; }
        public string? InsightHeading { get; set; }
        public string? InsightDescription { get; set; }
        public string? InsightImage { get; set; }
        public string? InsightPdfReport { get; set; }
        public string? NewsletterHeading { get; set; }
        public string? NewsletterScript { get; set; }
        public string? InvestorLogin { get; set; }
        public string? VimeoVideoUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? Email { get; set; }
        public string? OverviewPdf { get; set; }
        public DateTime? WebsiteUpdateDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? WebsiteUpdateDateStr { get; set; }
    }
}
