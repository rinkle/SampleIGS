using System;

namespace IGS.Models
{
    public class ExperiencePageMapping
    {
        public int Id { get; set; }
        public int? FK_ExperienceId { get; set; }
        public int? FK_PageId { get; set; }
        public decimal? DisplayOrder { get; set; }
    }
}
