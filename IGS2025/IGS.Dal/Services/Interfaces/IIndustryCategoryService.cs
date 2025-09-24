using IGS.Models.KeyLessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Dal.Services.Interfaces
{
    public interface IIndustryCategoryService
    {
        Task SaveIndustryCategoryAsync(List<GetIndustryCategory_Result> industryCategoryListItems);
    }
}
