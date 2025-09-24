using IGS.Models.KeyLessModels;

namespace IGS.ViewModels
{
    /// <summary>
    /// ViewModel for Industry and related Categories
    /// </summary>
    public class IndustryViewModel
    {
        public GetIndustry_Result Industry { get; set; } = new();
        public List<GetIndustryCategory_Result> IndustryCategory { get; set; } = new();

        public IndustryViewModel() { }

        public IndustryViewModel(
            GetIndustry_Result? industryResult,
            IEnumerable<GetIndustryCategory_Result>? allIndustryCategories = null,
            bool isAdmin = false)
        {
            // Ensure Industry is never null
            Industry = industryResult ?? new GetIndustry_Result();

            // Filter categories (active + named) and order
            IndustryCategory = allIndustryCategories?
                .Where(x => !string.IsNullOrEmpty(x.IndustryName) && x.IsActive == true)
                .OrderBy(o => o.DisplayOrder)
                .ToList()
                ?? new List<GetIndustryCategory_Result>();

            // Admin mode: add a placeholder row
            if (isAdmin)
            {
                IndustryCategory.Add(new GetIndustryCategory_Result { Id = 0 });
            }
        }
    }
}
