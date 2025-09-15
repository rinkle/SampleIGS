using Globalsetting;
using Microsoft.AspNetCore.Mvc;

namespace IGS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommonController : Controller
    {
        private readonly GlobalCookies _cookies;
        public CommonController(GlobalCookies cookies)
        {
            _cookies = cookies;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region menu settings
        [HttpPost]
        [Area("Admin")]
        public JsonResult UpdateSetting(string currentView)
        {
            string newValue = currentView == "Desktop" ? "Mobile" : "Desktop";

            _cookies.CheckDesktop = newValue; // update cookie

            return Json(new { currentView = newValue }); // ✅ use the value you just set
        }


        #endregion
    }
}
