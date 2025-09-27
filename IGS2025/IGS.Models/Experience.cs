using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IGS.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }
        public string? SupportText { get; set; }
        public string? TopLogo1 { get; set; }
        public string? TopLogo1Caption { get; set; }
        public string? TopLogo2 { get; set; }
        public string? TopLogo2Caption { get; set; }
        public string? Bottom1Logo { get; set; }
        public string? Bottom1LogoCaption { get; set; }
        public string? Bottom2Logo { get; set; }
        public string? Bottom2LogoCation { get; set; }
        public string? Target { get; set; }
        public string? PublishedDate { get; set; }   // ⚠️ consider DateTime? if this is really a date
        public DateTime? EndDate { get; set; }
        public string? Website { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? ExperienceUrl { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool? HideTombstone { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
