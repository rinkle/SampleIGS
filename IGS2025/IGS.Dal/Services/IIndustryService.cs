using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services
{
    public interface IIndustryService
    {
        /// <summary>
        /// Save or update an Industry record.
        /// </summary>
        Task SaveIndustryAsync(GetIndustry_Result IndustryItem);
    }
}
