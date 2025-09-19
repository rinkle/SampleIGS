using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("PageHeader")]
    public partial class PageHeader
    {
        [Key]
        public int Id { get; set; }
        public int? Fk_PageId { get; set; }
        public string? PageTitle { get; set; }
        public string? MetaData { get; set; }
        public string? MetaDescription { get; set; }
        public string? HeaderImage { get; set; }
        public string? HeaderCaption { get; set; }
        public string? Additionalinfo { get; set; }
        public string? AdditionalSubHeading { get; set; }
        public string? AdditionalSubDescription { get; set; }
        public string? AdditionalImage { get; set; }
        public string? VimeoVideoUrl { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
