using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;

namespace IGS.Dal.Services.Implementations
{
    public class TeamTitleService : ITeamTitleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public TeamTitleService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        public async Task<int> SaveTeamTitleAsync(GetTeamTitle_Result model)
        {
            if (model == null) return 0;

            try
            {
                TeamTitle entity;

                if (model.Id > 0)
                {
                    entity = await _unitOfWork.TeamTitle.GetAsync(x => x.Id == model.Id, tracked: true);

                    if (entity == null)
                    {
                        entity = new TeamTitle();
                        await _unitOfWork.TeamTitle.AddAsync(entity);
                    }

                    MapTeamTitle(entity, model);
                    entity.ModifiedBy = _env.UserId;
                    entity.ModifiedDate = DateTime.Now;

                    _unitOfWork.TeamTitle.Update(entity);
                }
                else
                {
                    entity = new TeamTitle();
                    MapTeamTitle(entity, model);

                    entity.CreatedBy = _env.UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.ModifiedBy = _env.UserId;
                    entity.ModifiedDate = DateTime.Now;

                    await _unitOfWork.TeamTitle.AddAsync(entity);
                }

                await _unitOfWork.SaveAsync();

                // Update URL (SP-based, like Experience)
                await _unitOfWork.TeamTitle.UpdateTeamTitleUrlAsync(entity.Id);

                return entity.Id;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in TeamTitleService.SaveTeamTitleAsync");
                throw;
            }
        }

        public async Task<bool> DeleteTeamTitleAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.TeamTitle.GetAsync(x => x.Id == id, tracked: true);

                if (entity == null) return false;

                entity.IsActive = false; // soft delete
                entity.ModifiedBy = _env.UserId;
                entity.ModifiedDate = DateTime.Now;

                _unitOfWork.TeamTitle.Update(entity);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.TeamTitle.UpdateTeamTitleUrlAsync(entity.Id);

                return true;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting TeamTitle {id}");
                throw;
            }
        }

        private static void MapTeamTitle(TeamTitle target, GetTeamTitle_Result src)
        {
            if (!string.IsNullOrWhiteSpace(src.Title))
                target.Title = src.Title.Trim();

            target.DisplayOrder = src.DisplayOrder;
            target.IsActive = src.IsActive ?? true;
        }
    }
}
