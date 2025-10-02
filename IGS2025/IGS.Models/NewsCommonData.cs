using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("NewsCommonData")]
    public class NewsCommonData
    {
        [Key]
        public int Id { get; set; }
        public string? InsightHeading { get; set; }
        public string? InsightSubHeading { get; set; }
        public string? FeaturedInsightHeading { get; set; }
        public string? FeaturedInsightSubHeading { get; set; }
        public string? FeaturedInsightDescription { get; set; }
        public string? FeaturedInsightImage { get; set; }
        public string? FeaturedInsightPdf { get; set; }

    }
}
