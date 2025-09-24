using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Globalsetting
{
    /// <summary>
    /// Wrapper for getting/setting cookies with local request caching.
    /// </summary>
    public class GlobalCookies
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Cache values for the lifetime of the current request
        private readonly Dictionary<string, string> _requestCache = new();

        public GlobalCookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

        /// <summary>
        /// Reads a cookie value safely. Returns "" if not found.
        /// </summary>
        private string GetCookie(string key)
        {
            if (_requestCache.TryGetValue(key, out var cached))
                return cached;

            if (HttpContext?.Request?.Cookies.TryGetValue(key, out var value) == true)
            {
                _requestCache[key] = value ?? string.Empty;
                return value ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets a cookie value and updates local cache.
        /// </summary>
        private void SetCookie(string key, string value, int expireDays = 1000)
        {
            if (HttpContext == null) return;

            _requestCache[key] = value;

            var options = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(expireDays),
                HttpOnly = true, // security best practice
                Secure = true,   // only over HTTPS
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            HttpContext.Response.Cookies.Append(key, value, options);
        }

        // -------------------------
        // Strongly typed properties
        // -------------------------

        public string CheckDesktop
        {
            get => GetCookie("CheckDesktop");
            set => SetCookie("CheckDesktop", value);
        }

        public string ShowMap
        {
            get => GetCookie("ShowMap");
            set => SetCookie("ShowMap", value);
        }

        public string UserName
        {
            get => GetCookie("UserName");
            set => SetCookie("UserName", value);
        }

        public string UserPassword
        {
            get => GetCookie("UserPassword");
            set => SetCookie("UserPassword", value);
        }

        public bool IsRemembered
        {
            get => bool.TryParse(GetCookie("IsRemembered"), out var result) && result;
            set => SetCookie("IsRemembered", value.ToString());
        }
    }
}
