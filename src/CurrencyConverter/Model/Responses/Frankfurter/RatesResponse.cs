using System.Text.Json.Serialization;

namespace CurrencyConverter.Model.Responses.Frankfurter;

public class RatesResponse : BaseFrankfurterResponse
{
    [JsonPropertyName("date")] 
    public string Date { get; set; }

    [JsonPropertyName("rates")] 
    public Dictionary<string, decimal> Rates { get; set; }
}