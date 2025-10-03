using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class PrivacyPolicyViewModel
    {
        public List<GetPrivacyPolicy_Result> Policies { get; set; } = new();

        public PrivacyPolicyViewModel() { }

        public PrivacyPolicyViewModel(IEnumerable<GetPrivacyPolicy_Result>? data, bool isAdmin = false)
        {
            Policies = data?.OrderBy(x => x.DisplayOrder).ToList() ?? new List<GetPrivacyPolicy_Result>();
        }
    }
}
