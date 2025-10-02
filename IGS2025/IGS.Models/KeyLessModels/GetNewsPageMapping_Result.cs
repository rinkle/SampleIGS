using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless]
    public class GetNewsPageMapping_Result
    {
        public int PageId { get; set; }
        public string? PageName { get; set; }
        public string? BodyPageId { get; set; }
        public string? PageUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool CheckedStatus { get; set; }
        public int? NewsId { get; set; }
    }
}
