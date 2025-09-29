using IGS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("TeamTitle")]
    public class TeamTitle
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string? TitleUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
