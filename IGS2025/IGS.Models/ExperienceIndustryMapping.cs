using System;

namespace IGS.Models
{
    public class ExperienceIndustryMapping
    {
        public int Id { get; set; }
        public int? Fk_ExperienceId { get; set; }
        public int? Fk_IndustryCategoryId { get; set; }
        public decimal? DisplayOrder { get; set; }
    }
}
