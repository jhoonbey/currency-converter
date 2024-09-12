using CurrencyConverter.Model.Requests;
using CurrencyConverter.Model.Responses;
using CurrencyConverter.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CurrencyConverter.Controllers;

/// <summary>
/// CurrencyConversionController
/// </summary>
/// <param name="currencyConversionService"></param>
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CurrencyConversionController(ICurrencyConversionService currencyConversionService) : BaseApiController
{
    /// <summary>
    ///  This will return latest exchange rates
    /// </summary>`
    [HttpGet("latest")]
    [SwaggerResponse(200, "", Type = typeof(LatestRatesResponse))]
    public async Task<IActionResult> GetLatestRatesAsync([FromQuery] LatestRatesRequest request)
    {
        var response = await currencyConversionService.GetLatestRatesAsync(request);
        return MapResponse(response);
    }

    /// <summary>
    ///  This will convert amounts between different currencies
    /// </summary>`
    [HttpPost("convert")]
    [SwaggerResponse(200, "", Type = typeof(ConversionResponse))]
    [SwaggerResponse(400, "", Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> GetConversionAsync(ConversionRequest request)
    {
        var response = await currencyConversionService.GetConversionAsync(request);
        return MapResponse(response);
    }

    /// <summary>
    ///  This will return a set of historical rates for a given period using pagination
    /// </summary>`
    [HttpGet("historical")]
    [SwaggerResponse(200, "", Type = typeof(HistoricalResponse))]
    public async Task<IActionResult> GetHistoricalRatesAsync([FromQuery] HistoricalRequest request)
    {
        var response = await currencyConversionService.GetHistoricalRatesAsync(request);
        return MapResponse(response);
    }
}