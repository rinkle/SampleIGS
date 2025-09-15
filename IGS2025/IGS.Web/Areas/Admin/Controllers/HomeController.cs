using Globalsetting;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Buffers.Text;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRole.Admin + "," + UserRole.SuperAdmin))]
    [RemoveCache]
    public class HomeController : BaseController
    {
        private readonly string baseUrl;
        public HomeController(IOptions<AppSettings> options)
        {
            baseUrl = options.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            SuccessNotification("Success");
            //ErrorNotification("Error found");
            return RedirectToAction("Contact1", "Home");

        }
        public IActionResult Contact1()
        {
            return View("Index");
        }
    }


}
