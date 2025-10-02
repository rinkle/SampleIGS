using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("NewsCategory")]
    public class NewsCategory
    {
        [Key]
        public int Id { get; set; }
        public int? FK_NewsId { get; set; }
        public int? FK_NewsCategoryId { get; set; }
        public decimal? DisplayOrder { get; set; }
    }
}
