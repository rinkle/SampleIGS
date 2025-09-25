using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // SP / View result (no primary key)
    public class GetExperienceDetail_Result
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }
        public string? TopLogo1 { get; set; }
        public string? TopLogo1Caption { get; set; }
        public string? TopLogo2 { get; set; }
        public string? TopLogo2Caption { get; set; }
        public string? Bottom1Logo { get; set; }
        public string? Bottom1LogoCaption { get; set; }
        public string? Bottom2Logo { get; set; }
        public string? Bottom2LogoCation { get; set; }
        public string? Target { get; set; }

        // Use DateTime? if it's a real date, otherwise keep string?
        public string? PublishedDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string? Website { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? ExperienceUrl { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool? HideTombstone { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
    }
}
