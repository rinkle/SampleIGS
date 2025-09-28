using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // Stored procedure / view result
    public class GetTeamCategory_Result
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string? Attribute { get; set; }
        public bool? IsActive { get; set; }
    }
}
