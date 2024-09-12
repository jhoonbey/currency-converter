namespace CurrencyConverter.Services;

public interface ICacheService
{
    Task<T?> GetDataAsync<T>(string key);

    Task SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
}