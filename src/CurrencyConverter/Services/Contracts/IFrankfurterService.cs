using CurrencyConverter.Model.Responses.Frankfurter;
using Refit;

namespace CurrencyConverter.Services;

public interface IFrankfurterService
{
    [Get("/latest")]
    Task<ApiResponse<RatesResponse>> GetLatestRatesAsync(string? from = "EUR");

    [Get("/latest")]
    Task<ApiResponse<RatesResponse>> GetConversionAsync(decimal amount, string from, string to);

    [Get("/{startDate}..{endDate}")]
    Task<ApiResponse<HistoricalRatesResponse>> GetHistoricalRatesAsync(string startDate, string endDate,
        [Query] string? to);
}