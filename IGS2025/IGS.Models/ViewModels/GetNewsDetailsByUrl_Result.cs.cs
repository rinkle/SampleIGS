using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless]
    public class GetNewsDetailsByUrl_Result
    {
        public int NewsId { get; set; }
        public DateTime? NewsDate { get; set; }
        public string? NewsHeadLine { get; set; }
        public string? Logo { get; set; }
        public string? SortDescription { get; set; }
        public string? KeyInsight { get; set; }
        public string? BottomText { get; set; }
        public string? Description { get; set; }
        public string? PdfFileName { get; set; }
        public string? NewsType { get; set; }
        public string? ExternalLink { get; set; }
        public string? NewsUrl { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Aggregated category list (comma-separated string)
        public string? CategoryNames { get; set; }

        // Navigation data (previous/next)
        public int? PreviousNewsId { get; set; }
        public string? PreviousNewsHeadLine { get; set; }
        public string? PreviousNewsUrl { get; set; }

        public int? NextNewsId { get; set; }
        public string? NextNewsHeadLine { get; set; }
        public string? NextNewsUrl { get; set; }
    }
}
