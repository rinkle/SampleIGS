using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class PrivacyPolicyService : IPrivacyPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public PrivacyPolicyService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        public async Task SavePrivacyPolicyAsync(PrivacyPolicyViewModel model, string pageName)
        {
            try
            {
                if (model == null || model.Policies.Count == 0) return;

                foreach (var policy in model.Policies)
                {
                    var entity = await _unitOfWork.PrivacyPolicy.GetAsync(p => p.Id == policy.Id, tracked: true);

                    if (entity != null)
                    {
                        entity.Title = policy.Title;
                        entity.Description = policy.Description;
                        entity.DisplayOrder = policy.DisplayOrder;
                        entity.PageName = pageName;
                        entity.ModifiedDate = DateTime.Now;
                        entity.ModifiedBy = _env.UserId;

                        _unitOfWork.PrivacyPolicy.Update(entity);
                    }
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in PrivacyPolicyService.SavePrivacyPolicyAsync");
                throw;
            }
        }
    }
}
