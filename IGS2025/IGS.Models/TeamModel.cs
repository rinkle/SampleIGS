using IGS.Models.KeyLessModels;

namespace IGS.Models
{
    /// <summary>
    /// Model for add/edit Team (single record + mappings).
    /// </summary>
    public class TeamModel
    {
        public GetTeamDetails_Result TeamInfo { get; set; } = new();
        public List<getTeamTeamCategoryMapping_Result> TeamCategoryMappings { get; set; } = new();

        // ✅ New property for dropdown or selection of titles
        public List<GetTeamTitle_Result> TeamTitleList { get; set; } = new();

        public TeamModel() { }

        public TeamModel(GetTeamDetails_Result? teamInfo,
                         IEnumerable<getTeamTeamCategoryMapping_Result>? categoryMappings,
                         IEnumerable<GetTeamTitle_Result>? teamTitles = null)
        {
            TeamInfo = teamInfo ?? new GetTeamDetails_Result();
            TeamCategoryMappings = categoryMappings?.ToList() ?? new List<getTeamTeamCategoryMapping_Result>();
            TeamTitleList = teamTitles?.ToList() ?? new List<GetTeamTitle_Result>();
        }
    }
}
