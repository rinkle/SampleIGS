using System;
using System.Security.Claims;
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

        // 🔹 Read AppSettings or build custom baseurl
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

        // 🔹 Current logged-in user ID
        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        // 🔹 Current logged-in username/email
        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        // 🔹 Check if logged in
        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LAKES ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
            };
                var tensMap = new[]
                {
                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
            };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public string ToStringWithSuffix(string day)
        {
            string suffix = "th";

            if (int.Parse(day) < 11 || int.Parse(day) > 20)
            {
                day = day.ToCharArray()[day.ToCharArray().Length - 1].ToString();
                switch (day)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }

            return suffix;
        }
    }

    public class AppSettings
    {
        public string BaseUrl { get; set; } = "";
        public string? AppVirtualDirectory { get; set; }
    }
}
