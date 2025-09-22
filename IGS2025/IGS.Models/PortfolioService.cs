using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PortfolioServices")] // ✅ match actual table name
    public class PortfolioService
    {
        [Key] // ✅ primary key
        public int Id { get; set; }
        public string? CoreAreasHeading { get; set; }
        public string? CoreAreasDescription { get; set; }
        public string? IndustryExpertiseHeading { get; set; }
        public string? IndustryExpertiseSubHeading { get; set; }
        public string? IndustryExpertiseDescription { get; set; }
        public string? FeaturedInsightHeading { get; set; }
        public string? FeaturedInsightSubHeading { get; set; }
        public string? FeaturedInsighDescription { get; set; }
        public string? FeaturedInsighImage { get; set; }
        public string? FeaturedInsighPdf { get; set; }
        public string? InsightHeading { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}
