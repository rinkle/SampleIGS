using IGS.Dal.Sql;
using IGS.Models.KeyLessModels;
using IGS.Models.ViewModels;

namespace IGS.Dal.Services
{
   
    public class PageHeaderService : IPageHeaderService
    {
        private readonly ISqlHelper _sql;

        public PageHeaderService(ISqlHelper sql)
        {
            _sql = sql;
        }

        public async Task<PageHeaderModel> GetPageHeaderAsync(string pageName, int pageId, CancellationToken ct = default)
        {
            // Call stored procedure GetPageHeader
            var header = await _sql.QuerySingleOrDefaultAsync<GetPageHeader_Result>(
                "GetPageHeader",
                new { PageName = pageName },
                isStoredProc: true,
                ct: ct
            );

            // Call stored procedure GetCommonListing for carousel
            var carousel = await _sql.QueryAsync<GetCommonListing_Result>(
                "GetCommonListing",
                new { PageId = pageId },
                isStoredProc: true,
                ct: ct
            );

            return new PageHeaderModel
            {
                PageHeader = header ?? new GetPageHeader_Result(),
                HeaderCarousel = carousel.ToList()
            };
        }
    }
}
