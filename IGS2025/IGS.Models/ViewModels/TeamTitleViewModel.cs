using IGS.Models.KeyLessModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGS.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Team Titles
    /// </summary>
    public class TeamTitleViewModel
    {
        public List<GetTeamTitle_Result> TeamTitleList { get; set; } = new();

        public TeamTitleViewModel() { }

        public TeamTitleViewModel(
            IEnumerable<GetTeamTitle_Result>? titleList = null,
            bool isAdmin = false)
        {
            // Ensure list is never null
            TeamTitleList = titleList?.ToList() ?? new List<GetTeamTitle_Result>();

            // Admin mode: add a placeholder row
            //if (isAdmin)
            //{
            //    TeamTitleList.Add(new GetTeamTitle_Result { Id = 0 });
            //}
        }
    }
}
