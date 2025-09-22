using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models
{
    [Table("Industries")]
    public class Industry
    {
        [Key] // ✅ Explicitly mark as primary key
        public int Id { get; set; }
        public string IndustryHeading { get; set; } = string.Empty;
        public string? IndustryDescription { get; set; }
        public string? InsightHeading { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
