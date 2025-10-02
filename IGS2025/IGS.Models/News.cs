using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("News")]
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        public DateTime NewsDate { get; set; }
        public string? NewsHeadLine { get; set; }
        public string? Logo { get; set; }
        public string? SortDescription { get; set; }
        public string? KeyInsight { get; set; }
        public string? BottomText { get; set; }
        public string? Description { get; set; }
        public string? PdfFileName { get; set; }
        public int? NewsType { get; set; }
        public string? ExternalLink { get; set; }
        public string? NewsUrl { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
