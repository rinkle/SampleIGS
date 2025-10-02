using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("NewsCategoryMapping")]
    public class NewsCategoryMapping
    {
        [Key]
        public int Id { get; set; }
        public int? FK_NewsId { get; set; }
        public int? FK_NewsCategoryId { get; set; }
        public decimal? DisplayOrder { get; set; }
    }
}
