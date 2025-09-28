using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // Stored procedure / view result
    public class getTeamTeamCategoryMapping_Result
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? Attribute { get; set; }
        public bool? IsActive { get; set; }
        public decimal DisplayOrder { get; set; }   // kept non-nullable since original wasn't Nullable
        public int CheckedStatus { get; set; }      // kept non-nullable since original wasn't Nullable
        public int? Fk_TeamId { get; set; }
    }
}
