namespace CurrencyConverter.Constants;

public static class ApplicationConstants
{
    public const int MaxAPIRetryCount = 5;
    public const double RedisExpirationInMinutes = 1440; // 1 day
    public const int DefaultPageSize = 10;
    public static readonly List<string> ExcludedCurrencies = ["TRY", "PLN", "THB", "MXN"];
}