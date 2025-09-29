using IGS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("TeamCategoryMapping")]
    public class TeamCategoryMapping
    {
        [Key]
        public int Id { get; set; }
        public int? Fk_TeamId { get; set; }
        public int? Fk_CategoryId { get; set; }
        public decimal? DisplayOrder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
