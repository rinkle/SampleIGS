using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class PrivacyPolicyVmService : IPrivacyPolicyVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public PrivacyPolicyVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PrivacyPolicyViewModel> GetPrivacyPolicyVmAsync(string pageName, bool isAdmin = false)
        {
            try
            {
                var policies = await _unitOfWork.PrivacyPolicy.GetPrivacyPolicyFromSpAsync(pageName);
                return new PrivacyPolicyViewModel(policies, isAdmin);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in PrivacyPolicyVmService.GetPrivacyPolicyVmAsync");
                return new PrivacyPolicyViewModel();
            }
        }
    }
}
