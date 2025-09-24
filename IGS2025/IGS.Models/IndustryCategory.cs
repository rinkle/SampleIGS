using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models
{
    [Table("IndustryCategory")]
    public class IndustryCategory
    {
        [Key] // ✅ Explicit PK
        public int Id { get; set; }
        public string? IndustryName { get; set; } = string.Empty;
        public string? IndustryDescription { get; set; }

        [Column(TypeName = "decimal(18,2)")] // ✅ Safe precision for SQL
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
