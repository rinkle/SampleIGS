using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // ✅ This makes it SP-only, no table creation
    public class GetIndustryCategory_Result
    {
        public int Id { get; set; }
        public string? IndustryName { get; set; }
        public string? IndustryDescription { get; set; }

        [Precision(18, 2)] // ✅ prevents decimal warnings
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
