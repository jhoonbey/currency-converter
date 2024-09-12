using CurrencyConverter.Model.Requests;
using CurrencyConverter.Model.Responses;

namespace CurrencyConverter.Services;

public interface ICurrencyConversionService
{
    Task<LatestRatesResponse> GetLatestRatesAsync(LatestRatesRequest request);

    Task<ConversionResponse> GetConversionAsync(ConversionRequest request);
    
    Task<HistoricalResponse> GetHistoricalRatesAsync(HistoricalRequest request);
}