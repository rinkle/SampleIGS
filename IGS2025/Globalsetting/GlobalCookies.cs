using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Globalsetting
{
    public class GlobalCookies
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // cache for values set in the current request
        private readonly Dictionary<string, string> _requestCache = new();

        public GlobalCookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        private string GetCookieVal(string key)
        {
            if (_requestCache.ContainsKey(key))
                return _requestCache[key]; // ✅ return latest set value immediately

            if (HttpContext == null) return string.Empty;

            HttpContext.Request.Cookies.TryGetValue(key, out var value);
            return value ?? string.Empty;
        }

        private void UpdateCookieVal(string key, string val, int expireDays)
        {
            if (HttpContext == null) return;

            _requestCache[key] = val ?? string.Empty; // ✅ update local cache too

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(expireDays),
                HttpOnly = false,
                Secure = false, // true if HTTPS
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            };

            HttpContext.Response.Cookies.Append(key, val ?? string.Empty, options);
        }

        // ✅ Properties

        public string CheckDesktop
        {
            get => GetCookieVal("CheckDesktop");
            set => UpdateCookieVal("CheckDesktop", value, 1000);
        }

        public string ShowMap
        {
            get => GetCookieVal("ShowMap");
            set => UpdateCookieVal("ShowMap", value, 1000);
        }

        public string UserName
        {
            get => GetCookieVal("UserName");
            set => UpdateCookieVal("UserName", value, 1000);
        }

        public string UserPassword
        {
            get => GetCookieVal("UserPassword");
            set => UpdateCookieVal("UserPassword", value, 1000);
        }

        public bool IsRemembered
        {
            get => bool.TryParse(GetCookieVal("IsRemembered"), out var result) && result;
            set => UpdateCookieVal("IsRemembered", value.ToString(), 1000);
        }
    }
}
