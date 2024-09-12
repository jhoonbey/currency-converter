using CurrencyConverter.Model.Requests;

namespace CurrencyConverter.Model.Responses;

public class HistoricalResponse : BaseResponse
{
    public decimal Amount { get; set; }
    public string Base { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
    
    // Pagination can be sent in Header too
    public PaginationModel Pagination { get; set; }
}