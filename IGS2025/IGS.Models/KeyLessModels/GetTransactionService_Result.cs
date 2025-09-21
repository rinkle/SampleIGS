using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGS.Models.KeyLessModels
{
    public class GetTransactionService_Result
    {
        public int Id { get; set; }
        public string? AreasofFocusHeading { get; set; }
        public string? AreasofFocusDescription { get; set; }
        public string? IndustryExpertiseHeading { get; set; }
        public string? IndustryExpertiseSubHeading { get; set; }
        public string? IndustryExpertiseDescription { get; set; }
        public string? RecentProjectHeading { get; set; }
        public string? RecentProjectDescription { get; set; }
        public string? InsightHeading { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }

}
