using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless]
    public class GetNewsCategoryMapping_Result
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Attribute { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool CheckedStatus { get; set; }
        public int? NewsId { get; set; }
    }
}
