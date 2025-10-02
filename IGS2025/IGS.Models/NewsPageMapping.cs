using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    [Table("NewsPageMapping")]
    public class NewsPageMapping
    {
        [Key]
        public int Id { get; set; }
        public int? FK_NewsId { get; set; }
        public int? FK_PageId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
