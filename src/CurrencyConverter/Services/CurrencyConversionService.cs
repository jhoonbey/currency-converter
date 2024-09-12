using System.Net;
using AutoMapper;
using CurrencyConverter.Constants;
using CurrencyConverter.Model.Requests;
using CurrencyConverter.Model.Responses;

namespace CurrencyConverter.Services;

public class CurrencyConversionService(
    IFrankfurterService frankfurterService,
    ICacheService cacheService,
    IMapper mapper,
    ILogger<CurrencyConversionService> logger)
    : ICurrencyConversionService
{
    public async Task<LatestRatesResponse> GetLatestRatesAsync(LatestRatesRequest request)
    {
        var frankfurterResponse = await frankfurterService.GetLatestRatesAsync(request.BaseCurrency);
        if (!frankfurterResponse.IsSuccessStatusCode || frankfurterResponse.Content == null)
        {
            return new LatestRatesResponse
            {
                // I return Bad Request for every status from external API, this mapping can be change
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{ValidationMessages.ApiCallError}. Status code: {frankfurterResponse.StatusCode}"
            };
        }

        var response = mapper.Map<LatestRatesResponse>(frankfurterResponse.Content);

        response.Success = true;
        return response;
    }

    public async Task<ConversionResponse> GetConversionAsync(ConversionRequest request)
    {
        // validation
        // excluded currencies can be stored in config file, DB etc.
        var excludedCurrencies = ApplicationConstants.ExcludedCurrencies;
        if (excludedCurrencies.Contains(request.FromCurrency) || excludedCurrencies.Contains(request.ToCurrency))
        {
            return new ConversionResponse
            {
                // I return Bad Request for every status from external API, this mapping can be change
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{ValidationMessages.DontUseCurrencyError}: {string.Join(",", excludedCurrencies)}"
            };
        }

        var frankfurterResponse =
            await frankfurterService.GetConversionAsync(request.Amount, request.FromCurrency, request.ToCurrency);
        if (!frankfurterResponse.IsSuccessStatusCode || frankfurterResponse.Content == null)
        {
            return new ConversionResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{ValidationMessages.ApiCallError}. Status code: {frankfurterResponse.StatusCode}"
            };
        }

        var response = mapper.Map<ConversionResponse>(frankfurterResponse.Content);

        response.Success = true;
        return response;
    }

    public async Task<HistoricalResponse> GetHistoricalRatesAsync(HistoricalRequest request)
    {
        // get all rates
        var allRatesResponse = await GetAllHistoricalRatesAsync(request);
        if (!allRatesResponse.Success)
        {
            return allRatesResponse;
        }

        var convertedRates = allRatesResponse.Rates
            .SelectMany(k => k.Value.Select(v => new RateItem
            {
                Date = k.Key,
                Currency = v.Key,
                Rate = v.Value
            }))
            .ToList();

        // Apply pagination
        var totalCount = convertedRates.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paginatedRates = convertedRates
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .GroupBy(rate => rate.Date)
            .ToDictionary(g => g.Key, g => g.ToDictionary(rate => rate.Currency, rate => rate.Rate));

        var response = new HistoricalResponse
        {
            Success = true,
            Amount = allRatesResponse.Amount,
            Base = allRatesResponse.Base,
            StartDate = allRatesResponse.StartDate,
            EndDate = allRatesResponse.EndDate,
            Rates = paginatedRates,
            Pagination = new PaginationModel
            {
                CurrentPage = request.PageNo,
                TotalPages = totalPages,
                PageSize = request.PageSize,
                TotalCount = totalCount
            }
        };

        return response;
    }

    private async Task<HistoricalResponse> GetAllHistoricalRatesAsync(HistoricalRequest request)
    {
        // get from Redis
        var redisKey = string.IsNullOrEmpty(request.BaseCurrency)
            ? $"{request.StartDate}-{request.EndDate}"
            : $"{request.StartDate}-{request.EndDate}-{request.BaseCurrency}";

        var responseFromRedis = await cacheService.GetDataAsync<HistoricalResponse>(redisKey);
        if (responseFromRedis is not null)
        {
            responseFromRedis.Success = true;
            return responseFromRedis;
        }

        // get from API
        var frankfurterResponse =
            await frankfurterService.GetHistoricalRatesAsync(request.StartDate, request.EndDate, request.BaseCurrency);
        if (!frankfurterResponse.IsSuccessStatusCode || frankfurterResponse.Content == null)
        {
            return new HistoricalResponse
            {
                // I return Bad Request for every status from external API, this mapping can be change
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Message = $"{ValidationMessages.ApiCallError}. Status code: {frankfurterResponse.StatusCode}"
            };
        }

        var responseFromApi = mapper.Map<HistoricalResponse>(frankfurterResponse.Content);

        // save to Redis
        var expirationTime = DateTimeOffset.Now.AddMinutes(ApplicationConstants.RedisExpirationInMinutes);
        await cacheService.SetDataAsync(redisKey, responseFromApi, expirationTime);

        responseFromApi.Success = true;
        return responseFromApi;
    }
}