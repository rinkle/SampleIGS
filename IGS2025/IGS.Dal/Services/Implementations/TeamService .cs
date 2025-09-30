using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _env;

        public TeamService(IUnitOfWork unitOfWork, ILoggerService logger, GlobalEnvironmentSetting env)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _env = env;
        }

        public async Task<int> SaveTeamAsync(TeamModel model)
        {
            if (model == null || model.TeamInfo == null) return 0;

            try
            {
                var incoming = model.TeamInfo;
                Team teamEntity;

                if (incoming.TeamId > 0)
                {
                    teamEntity = await _unitOfWork.Team.GetAsync(
                        t => t.TeamId == incoming.TeamId,
                        tracked: true);

                    if (teamEntity == null)
                    {
                        teamEntity = new Team();
                        await _unitOfWork.Team.AddAsync(teamEntity);
                    }

                    MapTeam(teamEntity, incoming);
                    teamEntity.ModifiedBy = _env.UserId;
                    teamEntity.ModifiedDate = DateTime.Now;

                    _unitOfWork.Team.Update(teamEntity);
                }
                else
                {
                    teamEntity = new Team();
                    MapTeam(teamEntity, incoming);

                    teamEntity.CreatedBy = _env.UserId;
                    teamEntity.CreatedDate = DateTime.Now;
                    teamEntity.ModifiedBy = _env.UserId;
                    teamEntity.ModifiedDate = DateTime.Now;

                    await _unitOfWork.Team.AddAsync(teamEntity);
                }

                // Save entity
                await _unitOfWork.SaveAsync();

                // ✅ Call SP to update TeamUrl (instead of trigger conflict)
                await _unitOfWork.Team.UpdateTeamUrlAsync(teamEntity.TeamId);

                // Replace Category Mappings
                if (model.TeamCategoryMappings?.Any() == true)
                    if (model.TeamCategoryMappings?.Any() == true)
                    {
                        var selected = model.TeamCategoryMappings
                            .Where(x => x.CheckedStatus == 1)
                            .Select(x => (x.Id, x.DisplayOrder)) // tuple: CategoryId + DisplayOrder
                            .ToList();

                        await _unitOfWork.Team.ReplaceCategoryMappingsAsync(teamEntity.TeamId, selected);
                    }

                return teamEntity.TeamId;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in TeamService.SaveTeamAsync");
                throw;
            }
        }

        private static void MapTeam(Team target, GetTeamDetails_Result src)
        {
            target.Fk_LocationId = src.Fk_LocationId;
            if (src.Fk_LocationId==0)
            {
                target.Fk_LocationId = 1;
            }
            target.Fk_TeamTitleId = src.Fk_TeamTitleId;
            target.FirstName = src.FirstName;
            target.MiddleName = src.MiddleName;
            target.LastName = src.LastName;
            target.Email = src.Email;
            target.OfficeNumber = src.OfficeNumber;
            target.PhoneNumber = src.PhoneNumber;
            target.LinkedInUrl = src.LinkedInUrl;
            target.BioImage = src.BioImage;
            target.GridImage = src.GridImage;
            target.HomeBioImage = src.HomeBioImage;
            target.Comments = src.Comments;
            target.SortDescription = src.SortDescription;
            target.Description = src.Description;
            target.EducationTitle = src.EducationTitle;
            target.EducationDescription = src.EducationDescription;
            target.ExperienceTitle = src.ExperienceTitle;
            target.ExperienceDescription = src.ExperienceDescription;
            target.ListOnHome = src.ListOnHome;
            target.DisplayOrder = src.DisplayOrder; // or DisplayOrder depending on schema
            target.VCard = src.VCard;
            target.IsActive = src.IsActive ?? true;
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            try
            {
                var team = await _unitOfWork.Team.GetAsync(
                    x => x.TeamId == id,
                    tracked: true);

                if (team == null)
                    return false;

                team.IsActive = false;
                team.ModifiedBy = _env.UserId;
                team.ModifiedDate = DateTime.Now;
                _unitOfWork.Team.Update(team);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.Team.UpdateTeamUrlAsync(team.TeamId);

                return true;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting Team {id}");
                throw;
            }
        }

        public async Task<bool> DeletePhotoAsync(int id, Action<Team> clearPhotoAction)
        {
            try
            {
                var team = await _unitOfWork.Team.GetAsync(
                    t => t.TeamId == id,
                    tracked: true);

                if (team == null)
                    return false;

                clearPhotoAction(team); // e.g. team.BioImage = null;

                team.ModifiedBy = _env.UserId;
                team.ModifiedDate = DateTime.Now;

                _unitOfWork.Team.Update(team);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, $"Error deleting photo for Team {id}");
                throw;
            }
        }
    }
}
