using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless]
    public class GetOtherContact_Result
    {
        public int Id { get; set; }
        public string? InvestorLogin { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? Email { get; set; }
        public string? OverviewPdf { get; set; }
        public string? CareersUrl { get; set; }

    }
}
