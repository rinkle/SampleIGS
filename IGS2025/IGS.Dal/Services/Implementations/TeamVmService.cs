using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class TeamVmService : ITeamVmService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamVmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TeamViewModel> GetTeamVmAsync(
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null,
            bool isAdmin = false)
        {
            var teams = await _unitOfWork.Team.GetTeamFilterListFromSpAsync(
                categoryIds,
                locationIds,
                orderBy);

            var categories = await _unitOfWork.TeamCategory.GetAllAsync();

            return new TeamViewModel(teams, categories, isAdmin);
        }

        public async Task<TeamModel> GetTeamModelAsync(
            int teamId,
            string? categoryIds = null,
            string? locationIds = null,
            string? orderBy = null)
        {
            var teamDetail = await _unitOfWork.Team.GetTeamDetailByIdAsync(
                teamId,
                categoryIds,
                locationIds,
                orderBy);

            var teamCategories = await _unitOfWork.Team.GetTeamCategoryMappingAsync(teamId);
            var teamTitles = await _unitOfWork.Team.GetTeamTitlesAsync();
            return new TeamModel(teamDetail, teamCategories, teamTitles);
        }
    }
}
