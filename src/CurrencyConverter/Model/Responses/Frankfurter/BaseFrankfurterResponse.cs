using System.Text.Json.Serialization;

namespace CurrencyConverter.Model.Responses.Frankfurter;

public class BaseFrankfurterResponse
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    [JsonPropertyName("base")]
    public string Base { get; set; }
}