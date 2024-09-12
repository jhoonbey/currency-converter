using AutoMapper;
using CurrencyConverter.Model.Responses;
using CurrencyConverter.Model.Responses.Frankfurter;

namespace CurrencyConverter.Mapping;

public class FrankfurterProfile : Profile
{
    public FrankfurterProfile()
    {
        CreateMap<RatesResponse, BaseRatesResponse>();
        CreateMap<RatesResponse, LatestRatesResponse>();
        CreateMap<RatesResponse, ConversionResponse>();
        CreateMap<HistoricalRatesResponse, HistoricalResponse>();
    }
}