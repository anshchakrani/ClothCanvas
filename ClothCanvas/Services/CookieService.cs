using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClothCanvas.Services;

public class CookieService: ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public T GetFromCookie<T>(string key)
    {
        var value = _httpContextAccessor.HttpContext.Request.Cookies[key];

        return value != null ? JsonConvert.DeserializeObject<T>(value) : default(T);
    }

    public void SetCookie<T>(string key, T value, int? expireTime)
    {
        var options = new CookieOptions
        {
            Expires = expireTime.HasValue ? DateTime.Now.AddMinutes(expireTime.Value) : DateTime.Now.AddHours(1),
            IsEssential = true // The cookie will be stored regardless of the user's consent
        };

        var stringValue = JsonConvert.SerializeObject(value);

        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, stringValue, options);
    }
}
