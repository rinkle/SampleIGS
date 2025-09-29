using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class TeamTitleVmService : ITeamTitleVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public TeamTitleVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TeamTitleViewModel> GetTeamTitleVmAsync(bool isAdmin = false)
        {
            try
            {
                var titles = (await _unitOfWork.TeamTitle.GetTeamTitleListAsync()).ToList();
                return new TeamTitleViewModel(titles, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in TeamTitleVmService.GetTeamTitleVmAsync");
                return new TeamTitleViewModel();
            }
        }

        public async Task<GetTeamTitle_Result> GetTeamTitleDetailAsync(int id)
        {
            var detail = await _unitOfWork.TeamTitle.GetTeamTitleDetailByIdAsync(id);
            return detail ?? new GetTeamTitle_Result();
        }
    }
}
