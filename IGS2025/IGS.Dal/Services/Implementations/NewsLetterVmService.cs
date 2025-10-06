using IGS.Dal.Repository.IRepository;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services.Implementations
{
    public class NewsLetterVmService : INewsLetterVmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;

        public NewsLetterVmService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<NewsLetterViewModel> GetNewsLetterViewModelAsync()
        {
            try
            {
                var newsLetterInfo = await _unitOfWork.Home.GetHomeFromSpAsync();
                return new NewsLetterViewModel(newsLetterInfo);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex, "Error in NewsLetterVmService.GetNewsLetterViewModelAsync");
                return new NewsLetterViewModel();
            }
        }
    }
}
