namespace ClothCanvas.Services;

public interface ICookieService
{
    T GetFromCookie<T>(string key);
    void SetCookie<T>(string key, T value, int? expireTime);
}
