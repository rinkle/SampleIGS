using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // because this is an SP result, not a mapped table
    public class GetTeamFilterList_Result
    {
        public int TeamId { get; set; }
        public int Fk_LocationId { get; set; }
        public int Fk_TeamTitleId { get; set; }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? OfficeNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? BioImage { get; set; }
        public string? GridImage { get; set; }
        public string? HomeBioImage { get; set; }
        public string? Comments { get; set; }
        public string? SortDescription { get; set; }
        public string? Description { get; set; }
        public string? EducationTitle { get; set; }
        public string? EducationDescription { get; set; }
        public string? ExperienceTitle { get; set; }
        public string? ExperienceDescription { get; set; }
        public bool? ListOnHome { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? VCard { get; set; }
        public string? TeamUrl { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Aggregates
        public string? CategoryNames { get; set; }
        public string? LocationName { get; set; }
        public string? TeamTitleName { get; set; }

        // Navigation
        public int? PreviousId { get; set; }
        public string? PreviousName { get; set; }
        public int? NextId { get; set; }
        public string? NextName { get; set; }
    }
}
