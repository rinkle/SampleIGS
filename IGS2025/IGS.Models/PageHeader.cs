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

        [MaxLength(500)]
        public string? PageTitle { get; set; }

        [MaxLength(2000)]
        public string? MetaData { get; set; }

        [MaxLength(2000)]
        public string? MetaDescription { get; set; }

        public string? HeaderImage { get; set; }
        public string? HeaderCaption { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? AdditionalSubHeading { get; set; }
        public string? AdditionalSubDescription { get; set; }
        public string? AdditionalImage { get; set; }

        [MaxLength(450)] // matches IdentityUser PK length
        public string? CreatedBy { get; set; }

        [MaxLength(450)]
        public string? ModifiedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // ✅ Relationships (optional)
        [ForeignKey(nameof(Fk_PageId))]
        public virtual Page? Page { get; set; }
    }
}
