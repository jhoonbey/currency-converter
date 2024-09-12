using System.Text.Json.Serialization;

namespace CurrencyConverter.Model.Responses.Frankfurter;

public class HistoricalRatesResponse : BaseFrankfurterResponse
{
    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("end_date")] 
    public string EndDate { get; set; }

    [JsonPropertyName("rates")] 
    public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
}