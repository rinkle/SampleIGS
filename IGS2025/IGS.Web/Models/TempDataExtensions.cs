using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace IGS.Web
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T? Get<T>(this ITempDataDictionary tempData, string key)
        {
            if (!tempData.TryGetValue(key, out var o) || o == null)
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(o.ToString()!);
            }
            catch
            {
                // Fallback: handle plain strings stored instead of JSON
                if (typeof(T) == typeof(List<string>))
                {
                    return (T)(object)new List<string> { o.ToString()! };
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)o.ToString()!;
                }

                throw; // if it's something unexpected, rethrow
            }
        }
    }
}
