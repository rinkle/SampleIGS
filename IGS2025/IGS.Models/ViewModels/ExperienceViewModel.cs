using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Experience page.
    /// </summary>
    public class ExperienceViewModel
    {
        public List<GetExperienceFilterList_Result> ExperienceList { get; set; } = new();
        /// <summary>
        /// Industry categories are visible only to non-admin users.
        /// </summary>
        public List<GetIndustryCategory_Result> IndustryCategory { get; set; } = new();

        public ExperienceViewModel() { }

        public ExperienceViewModel(IEnumerable<GetExperienceFilterList_Result>? experienceList = null,
                                   IEnumerable<GetIndustryCategory_Result>? industryCategory = null,
                                   bool isAdmin = false)
        {
            // materialize + order like CoreAreasOfFocus pattern
            ExperienceList = experienceList?
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.ClientName)
                .ToList()
                ?? new List<GetExperienceFilterList_Result>();

            // always fetched, but hide for admins by keeping it empty
            if (!isAdmin)
            {
                IndustryCategory = industryCategory?
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
                    ?? new List<GetIndustryCategory_Result>();
            }
        }
    }
}
