using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("TeamCategory")]
    public class TeamCategory
    {
        [Key]
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? Attribute { get; set; }
        public bool? IsActive { get; set; }
    }
}
