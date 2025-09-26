using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // EF Core: tells EF this is a stored proc result with no PK
    public class GetExperiencePageMapping_Result
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? BodyPageId { get; set; }
        public string? PageUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool CheckedStatus { get; set; }   // ✅ now a bool
        public int? FK_ExperienceId { get; set; }
    }

}
