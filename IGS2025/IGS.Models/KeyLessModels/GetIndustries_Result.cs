using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // ✅ EF Core will NOT try to create a table
    public class GetIndustries_Result
    {
        public int Id { get; set; }
        public string? IndustryHeading { get; set; }
        public string? IndustryDescription { get; set; }
        public string? InsightHeading { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
