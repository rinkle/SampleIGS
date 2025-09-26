using IGS.Models.KeyLessModels;

namespace IGS.Models
{
    /// <summary>
    /// Model for add/edit Experience (single record + mappings).
    /// </summary>
    public class ExperienceModel
    {
        public GetExperienceDetail_Result ExperienceInfo { get; set; } = new();
        public List<GetExperienceIndustryCategoryMapping_Result> ExperienceIndustryCategoryMapping { get; set; } = new();
        public List<GetExperiencePageMapping_Result> ExperiencePageMapping { get; set; } = new();

        public ExperienceModel() { }

        public ExperienceModel(GetExperienceDetail_Result? experienceInfo,
                               IEnumerable<GetExperienceIndustryCategoryMapping_Result>? industryMappings,
                               IEnumerable<GetExperiencePageMapping_Result>? pageMappings)
        {
            ExperienceInfo = experienceInfo ?? new GetExperienceDetail_Result();
            ExperienceIndustryCategoryMapping = industryMappings?.ToList() ?? new List<GetExperienceIndustryCategoryMapping_Result>();
            ExperiencePageMapping = pageMappings?.ToList() ?? new List<GetExperiencePageMapping_Result>();
        }
    }
}
