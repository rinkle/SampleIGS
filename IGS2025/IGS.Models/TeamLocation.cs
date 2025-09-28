using IGS.Models.IGS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("TeamLocation")]
    public class TeamLocation
    {
        [Key]
        public int Id { get; set; }
        public string? LocationName { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string? LocationUrl { get; set; }

    }
}
