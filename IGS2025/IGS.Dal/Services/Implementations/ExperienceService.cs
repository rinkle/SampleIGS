using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;                    // Entity
using IGS.Models.KeyLessModels;      // SP result
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class ExperienceService : IExperienceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public ExperienceService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        public async Task<int> SaveExperienceAsync(ExperienceModel model)
        {
            if (model == null || model.ExperienceInfo == null) return 0;

            try
            {
                var incoming = model.ExperienceInfo;
                Experience expEntity;

                if (incoming.Id > 0)
                {
                    expEntity = await _unitOfWork.Experience.GetAsync(
                        e => e.Id == incoming.Id, tracked: true);

                    if (expEntity == null)
                    {
                        expEntity = new Experience();
                        await _unitOfWork.Experience.AddAsync(expEntity);
                    }

                    MapExperience(expEntity, incoming);
                    expEntity.ModifiedBy = _env.UserId;
                    expEntity.ModifiedDate = DateTime.Now;

                    _unitOfWork.Experience.Update(expEntity);
                }
                else
                {
                    expEntity = new Experience();
                    MapExperience(expEntity, incoming);

                    expEntity.CreatedBy = _env.UserId;
                    expEntity.CreatedDate = DateTime.Now;
                    expEntity.ModifiedBy = _env.UserId;
                    expEntity.ModifiedDate = DateTime.Now;

                    await _unitOfWork.Experience.AddAsync(expEntity);
                }

                // Save entity
                await _unitOfWork.SaveAsync();

                // ✅ Run the SP to update ExperienceUrl
                await _unitOfWork.Experience.UpdateExperienceUrlAsync(expEntity.Id);

                // Replace mappings
                if (model.ExperienceIndustryCategoryMapping?.Any() == true)
                {
                    var selectedIndustryIds = model.ExperienceIndustryCategoryMapping
                        .Where(x => x.CheckedStatus)
                        .Select(x => x.Id)
                        .ToList();

                    await _unitOfWork.Experience.ReplaceIndustryMappingsAsync(expEntity.Id, selectedIndustryIds);
                }

                if (model.ExperiencePageMapping?.Any() == true)
                {
                    var selectedPageIds = model.ExperiencePageMapping
                        .Where(x => x.CheckedStatus)
                        .Select(x => x.Id)
                        .ToList();

                    await _unitOfWork.Experience.ReplacePageMappingsAsync(expEntity.Id, selectedPageIds);
                }

                return expEntity.Id;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in ExperienceService.SaveExperienceAsync");
                throw;
            }
        }

        private static void MapExperience(Experience target, GetExperienceDetail_Result src)
        {
            target.ClientName = src.ClientName;
            target.SupportText= src.SupportText;
            target.TopLogo1 = src.TopLogo1;
            target.TopLogo1Caption = src.TopLogo1Caption;
            target.TopLogo2 = src.TopLogo2;
            target.TopLogo2Caption = src.TopLogo2Caption;
            target.Bottom1Logo = src.Bottom1Logo;
            target.Bottom1LogoCaption = src.Bottom1LogoCaption;
            target.Bottom2Logo = src.Bottom2Logo;
            target.Bottom2LogoCation = src.Bottom2LogoCation; // adjust spelling if needed
            target.Target = src.Target;
            target.PublishedDate = src.PublishedDate;
            target.EndDate = src.EndDate;
            target.Website = src.Website;
            target.DisplayOrder = src.DisplayOrder;
            target.TransactionDate = src.TransactionDate;
            target.HideTombstone = src.HideTombstone;
            target.IsActive = src.IsActive ?? true;
        }
    }
}
