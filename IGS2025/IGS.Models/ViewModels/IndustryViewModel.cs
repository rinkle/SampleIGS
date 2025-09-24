using Globalsetting;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class IndustryViewModel
    {
        public GetIndustry_Result Industry { get; set; } = new();
        public List<GetIndustryCategory_Result> IndustryCategory { get; set; } = new();

        public IndustryViewModel() { }

        public IndustryViewModel(GetIndustry_Result? industryResult, IEnumerable<GetIndustryCategory_Result>? allIndustryCategories = null, bool isAdmin = false)
        {
            // Ensure Industry is never null
            Industry = industryResult ?? new GetIndustry_Result();
            // Filter only active categories with valid names
            IndustryCategory = allIndustryCategories?.Where(x => !string.IsNullOrEmpty(x.IndustryName) && x.IsActive == true)
                .OrderBy(o => o.DisplayOrder).ToList() ?? new List<GetIndustryCategory_Result>();
            // Admin mode: allow adding a placeholder row
            if (isAdmin)
            {
                IndustryCategory.Add(new GetIndustryCategory_Result { Id = 0 });
            }
        }
    }
}
