using Microsoft.EntityFrameworkCore;

namespace IGS.Models.KeyLessModels
{
    [Keyless] // SP result, not a tracked entity
    public class GetTeamTitle_Result
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string? TitleUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
