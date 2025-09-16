namespace IGS.Models.KeyLessModels
{
    // DTO for stored procedure "GetHome"
    public class GetHome_Result
    {
        public int Id { get; set; }

        public string ExceptionalPortfolioHeading { get; set; } = string.Empty;
        public string Disclaimer { get; set; } = string.Empty;
        public string PortfolioBackgroundImage { get; set; } = string.Empty;
        public string NewsHeading { get; set; } = string.Empty;
        public string NewsBackGroundImage { get; set; } = string.Empty;
        public string InvestorLogin { get; set; } = string.Empty;
        public string VimeoVideoUrl { get; set; } = string.Empty;
        public string LinkedInUrl { get; set; } = string.Empty;
        public string TwitterUrl { get; set; } = string.Empty;
        public string FBUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string OverviewPdf { get; set; } = string.Empty;

        public DateTime? WebsiteUpdateDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;

        public string WebsiteUpdateDateStr { get; set; } = string.Empty;
    }
}
