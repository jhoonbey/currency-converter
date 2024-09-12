using System.Net;
using AutoMapper;
using CurrencyConverter.Model.Requests;
using CurrencyConverter.Model.Responses;
using CurrencyConverter.Model.Responses.Frankfurter;
using CurrencyConverter.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;

namespace CurrencyConverter.Tests;

public class CurrencyConversionServiceTests
{
    private readonly Mock<IFrankfurterService> _mockFrankfurterService;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CurrencyConversionService>> _mockLogger;
    private readonly CurrencyConversionService _service;

    public CurrencyConversionServiceTests()
    {
        _mockFrankfurterService = new Mock<IFrankfurterService>();
        _mockCacheService = new Mock<ICacheService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CurrencyConversionService>>();

        _service = new CurrencyConversionService(
            _mockFrankfurterService.Object,
            _mockCacheService.Object,
            _mockMapper.Object,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task GetLatestRatesAsync_ShouldReturnMappedResponse_WhenApiCallIsSuccessful()
    {
        // Arrange
        var request = new LatestRatesRequest { BaseCurrency = "USD" };
        var ratesResponse = new RatesResponse
        {
            Amount = 100,
            Base = "USD",
            Date = "2022-01-01",
            Rates = new Dictionary<string, decimal>()
        };
        var apiResponse =
            new ApiResponse<RatesResponse>(new HttpResponseMessage(HttpStatusCode.OK), ratesResponse,
                new RefitSettings());
        _mockFrankfurterService.Setup(s => s.GetLatestRatesAsync(request.BaseCurrency)).ReturnsAsync(apiResponse);

        var mappedResponse = new LatestRatesResponse { Success = true };
        _mockMapper.Setup(m => m.Map<LatestRatesResponse>(It.IsAny<RatesResponse>())).Returns(mappedResponse);

        // Act
        var result = await _service.GetLatestRatesAsync(request);

        // Assert
        result.Should().BeEquivalentTo(mappedResponse);
    }

    [Fact]
    public async Task GetConversionAsync_ShouldReturnBadRequest_WhenApiCallFails()
    {
        // Arrange
        var request = new ConversionRequest { Amount = 100, FromCurrency = "USD", ToCurrency = "EUR" };
        var apiResponse = new ApiResponse<RatesResponse>(new HttpResponseMessage(HttpStatusCode.BadRequest), null,
            new RefitSettings());

        _mockFrankfurterService
            .Setup(s => s.GetConversionAsync(request.Amount, request.FromCurrency, request.ToCurrency))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _service.GetConversionAsync(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ShouldReturnPaginatedResponse_WhenApiCallIsSuccessful()
    {
        // Arrange
        var request = new HistoricalRequest
        {
            BaseCurrency = "PLN",
            StartDate = "2020-01-01",
            EndDate = "2020-01-31",
            PageNo = 1,
            PageSize = 10
        };

        var historicalRatesResponse = new HistoricalRatesResponse
        {
            Amount = 100,
            Base = "PLN",
            StartDate = "2020-01-01",
            EndDate = "2020-01-31",
            Rates = new Dictionary<string, Dictionary<string, decimal>>()
            {
                { "2022-01-01", new Dictionary<string, decimal> { { "USD", 1.12m }, { "EUR", 1.0m } } },
                { "2023-01-02", new Dictionary<string, decimal> { { "USD", 4.22m }, { "EUR", 2.0m } } }
            }
        };

        var apiResponse =
            new ApiResponse<HistoricalRatesResponse>(new HttpResponseMessage(HttpStatusCode.OK),
                historicalRatesResponse,
                new RefitSettings());

        _mockFrankfurterService
            .Setup(s => s.GetHistoricalRatesAsync(request.StartDate, request.EndDate, request.BaseCurrency))
            .ReturnsAsync(apiResponse);

        var mappedResponse = new HistoricalResponse
        {
            Success = true,
            Amount = historicalRatesResponse.Amount,
            Base = historicalRatesResponse.Base,
            StartDate = historicalRatesResponse.StartDate,
            EndDate = historicalRatesResponse.EndDate,
            Rates = historicalRatesResponse.Rates.SelectMany(h => h.Value.Select(r => new RateItem
                {
                    Date = h.Key,
                    Currency = r.Key,
                    Rate = r.Value
                }))
                .GroupBy(rate => rate.Date)
                .ToDictionary(g => g.Key, g => g.ToDictionary(rate => rate.Currency, rate => rate.Rate)),
            Pagination = new PaginationModel
            {
                CurrentPage = request.PageNo,
                TotalPages = 1,
                PageSize = request.PageSize,
                TotalCount = historicalRatesResponse.Rates.Values.SelectMany(dict => dict.Values).Count()
            }
        };

        _mockMapper.Setup(m => m.Map<HistoricalResponse>(It.IsAny<HistoricalRatesResponse>()))
            .Returns(mappedResponse);

        // Act
        var result = await _service.GetHistoricalRatesAsync(request);

        // Assert
        result.Should().BeEquivalentTo(mappedResponse);
        result.Pagination.TotalCount.Should().BeGreaterThan(0);
    }
}