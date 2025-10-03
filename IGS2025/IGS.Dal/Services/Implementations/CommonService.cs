using Globalsetting;
using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironment;

        public CommonService(
            IUnitOfWork unitOfWork,
            ILoggerService logger,
            GlobalEnvironmentSetting globalEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _globalEnvironment = globalEnvironment;
        }

        public async Task<CommonHeaderFooterModel> GetCommonServiceAsync(string pageName)
        {
            try
            {
                var header = await _unitOfWork.Common.GetPageHeaderAsync(pageName);
                IEnumerable<GetContact_Result> contact = await _unitOfWork.Contact.GetContactFromSpAsync();
                if (contact == null)
                {
                    contact = new List<GetContact_Result>();
                }
                var otherContact = await _unitOfWork.Common.GetOtherContactAsync();

                return new CommonHeaderFooterModel
                {
                    PageName = pageName,
                    HeaderInfo = header,
                    OtherContact = otherContact,
                    ContactInfo = contact?.FirstOrDefault()
                };
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in CommonService.GetCommonHeaderAsync");
                throw;
            }
        }
    }
}
