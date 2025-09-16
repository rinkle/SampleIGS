using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("Page")]
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? BodyPageId { get; set; }
        public string? PageUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
