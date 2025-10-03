using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models.KeyLessModels
{
    [Keyless]
    public class GetPrivacyPolicy_Result
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? PageName { get; set; }
    }
}
