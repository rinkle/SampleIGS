using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class CommonHeaderFooterModel
    {
        public GetPageHeader_Result? HeaderInfo { get; set; }
        public GetContact_Result? ContactInfo { get; set; }
        public GetOtherContact_Result? OtherContact { get; set; }
        public string PageName { get; set; } = string.Empty;
    }
}
