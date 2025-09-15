// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Globalsetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IGS.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly GlobalEnvironmentSetting _globalEnvironmentSetting;
        private readonly string baseUrl;
        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, GlobalEnvironmentSetting globalEnvironmentSetting)
        {
            _signInManager = signInManager;
            _logger = logger;
            signInManager.SignOutAsync();
            _globalEnvironmentSetting = globalEnvironmentSetting;
            baseUrl = _globalEnvironmentSetting.AppSettingValue("AppSettings:baseurl");

        }
        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            // Some people want logout on GET too (optional)
            await _signInManager.SignOutAsync();
            return Redirect(baseUrl);
        }
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
