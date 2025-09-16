namespace IGS.Models.ViewModels
{
    public class HomeViewModel
    {
        public GetHome_Result? Home { get; set; }

        public HomeViewModel() { }

        public HomeViewModel(GetHome_Result? homeResult)
        {
            Home = homeResult;
        }
    }
}
