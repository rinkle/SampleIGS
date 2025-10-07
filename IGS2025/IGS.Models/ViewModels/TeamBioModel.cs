using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class TeamBioModel
    {
        public GetTeamDetails_Result TeamBio { get; set; } = new();
        public TeamBioModel()
        {

        }
        public TeamBioModel(GetTeamDetails_Result _teamBio)
        {
            TeamBio = _teamBio;
        }
    }
}
