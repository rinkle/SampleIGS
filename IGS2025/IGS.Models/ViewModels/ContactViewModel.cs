using IGS.Models.KeyLessModels;

namespace IGS.Models.ViewModels
{
    public class ContactViewModel
    {
        public List<GetContact_Result> ContactList { get; set; } = new();

        public ContactViewModel() { }

        public ContactViewModel(IEnumerable<GetContact_Result>? contactList = null)
        {
            ContactList = contactList?.ToList() ?? new List<GetContact_Result>();
        }
    }
}
