using Globalsetting;
using IGS.Dal.Services.Interfaces;
using IGS.Models.ViewModels;
using IGS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IGS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = (UserRoles.Admin + "," + UserRoles.SuperAdmin))]
    [RemoveCache]
    public class ContactController : BaseController
    {
        private readonly string _baseUrl;
        private readonly ILoggerService _logger;
        private readonly IContactVmService _contactVmService;
        private readonly IContactService _contactService;

        public ContactController(
            IOptions<AppSettings> options,
            ILoggerService logger,
            IContactVmService contactVmService,
            IContactService contactService)
        {
            _baseUrl = options.Value.BaseUrl;
            _logger = logger;
            _contactVmService = contactVmService;
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = await _contactVmService.GetContactVmAsync(true);
                return View(vm);
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Contact/Index");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
                return View(new ContactViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveContact(ContactViewModel model)
        {
            try
            {
                if (model != null && model.ContactList.Any())
                {
                    await _contactService.SaveContactAsync(model);
                    SuccessNotification("Contact addresses saved successfully!");
                }
            }
            catch (Exception ex)
            {
                int errorId = await _logger.LogErrorAsync(ex, "Error in Contact/SaveContact");
                ErrorNotification($"Something went wrong. Error ID: {errorId}");
            }

            return Redirect(_baseUrl + "admin/contact/");
        }
    }
}
