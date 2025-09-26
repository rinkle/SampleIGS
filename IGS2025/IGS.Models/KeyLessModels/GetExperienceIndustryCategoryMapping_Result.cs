using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // EF Core: this result model has no primary key
    public class GetExperienceIndustryCategoryMapping_Result
    {
        public int Id { get; set; }
        public string? IndustryName { get; set; }
        public string? IndustryDescription { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public bool CheckedStatus { get; set; }   // ✅ now a bool
        public int? Fk_ExperienceId { get; set; }
    }

}
