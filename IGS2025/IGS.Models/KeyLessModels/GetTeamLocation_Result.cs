using System;
using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // Stored procedure / view result
    public class GetTeamLocation_Result
    {
        public int Id { get; set; }
        public string? LocationName { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string? LocationUrl { get; set; }
    }
}
