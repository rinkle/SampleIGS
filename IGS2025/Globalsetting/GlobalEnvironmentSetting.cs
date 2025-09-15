using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;



namespace Globalsetting
{
    public class GlobalEnvironmentSetting
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GlobalEnvironmentSetting(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string AppSettingValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "/";

            try
            {
                if (key.Equals("AppSettings:baseurl", StringComparison.OrdinalIgnoreCase))
                {
                    var request = _httpContextAccessor.HttpContext?.Request;

                    if (request == null)
                        return "/";

                    // Build base URL (scheme://host[:port]/)
                    var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}/";

                    if (baseUrl.Contains("cleverdesign.com", StringComparison.OrdinalIgnoreCase))
                    {
                        var appVirtualDirectory = _configuration["AppSettings:AppVirtualDirectory"];
                        return "/" + (appVirtualDirectory ?? string.Empty).ToLower() + "/";
                    }
                    else
                    {
                        return "/";
                    }
                }
                else
                {
                    return _configuration[key] ?? "/";
                }
            }
            catch
            {
                return "/";
            }
        }
    }
}