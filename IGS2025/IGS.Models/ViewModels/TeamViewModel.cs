using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Team page.
    /// </summary>
    public class TeamViewModel
    {
        public List<GetTeamFilterList_Result> TeamList { get; set; } = new();
        /// <summary>
        /// Team categories are hidden for admins (same as IndustryCategory in Experience).
        /// </summary>
        public List<TeamCategory> TeamCategories { get; set; } = new();

        public TeamViewModel() { }

        public TeamViewModel(IEnumerable<GetTeamFilterList_Result>? teamList = null, IEnumerable<TeamCategory>? categories = null, bool isAdmin = false)
        {
            // SP already returns ordered results
            TeamList = teamList?.ToList() ?? new List<GetTeamFilterList_Result>();
            // only show categories if NOT admin
            if (!isAdmin)
            {
                TeamCategories = categories?.ToList() ?? new List<TeamCategory>();
            }
        }
    }
}
