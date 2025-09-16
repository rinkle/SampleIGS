using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Home")]
public partial class Home
{
    public int Id { get; set; }

    public string? ExceptionalPortfolioHeading { get; set; }
    public string? Disclaimer { get; set; }
    public string? PortfolioBackgroundImage { get; set; }
    public string? NewsHeading { get; set; }
    public string? NewsBackGroundImage { get; set; }
    public string? InvestorLogin { get; set; }
    public string? VimeoVideoUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? FBUrl { get; set; }
    public string? Email { get; set; }
    public string? OverviewPdf { get; set; }

    public DateTime? WebsiteUpdateDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    public virtual IdentityUser? CreatedByUser { get; set; }
    public virtual IdentityUser? ModifiedByUser { get; set; }
}
